using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FDownl_Shared_Resources;
using FDownl_Shared_Resources.Helpers;
using FDownl_Shared_Resources.Models;
using Ionic.Zip;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fdownl_Storage.Controllers
{
    public class UploadController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UploadController> _logger;
        private readonly DatabaseContext _databaseContext;

        public UploadController(IConfiguration configuration, ILogger<UploadController> logger, DatabaseContext databaseContext)
        {
            _configuration = configuration;
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public class UploadForm
        {
            [Required]
            public List<IFormFile> Files { get; set; }
            [Required]
            [Range(60, 604800)]
            public int Lifetime { get; set; }
            public string Code { get; set; }
        }

        public class UploadResult
        {
            public int Code { get; set; }
            public string Id { get; set; }
            public string Error { get; set; }
        }

        [HttpPost]
        [Route("/upload")]
        [EnableCors]
        [RequestSizeLimit(50 * 1024 * 1024)] // 50MB = 50 * 1024 * 1024
        public async Task<IActionResult> Index(UploadForm uploadForm)
        {
            if (!ModelState.IsValid)
                return Json(new UploadResult { Code = 1, Error = "Invalid options supplied!" });

            if (await IsRateLimited())
                return Json(new UploadResult { Code = 2, Error = "You are uploading too many files too fast. Please try again later." });

            if (await IsStorageFull())
                return Json(new UploadResult { Code = 3, Error = "The server you specified is full. Please choose another." });

            return Json(new UploadResult { Id = await UploadAsync(uploadForm) });
        }

        private async Task<bool> IsRateLimited()
        {
            string ip = HttpContext.Request.Headers["X-Forwarded-For"];
            var uploadedFiles = await _databaseContext.UploadedFiles
                .Where(x => x.Ip == ip)
                .ToListAsync();
            var uploadedFilesLastMinute = uploadedFiles
                .Where(x => DateTime.UtcNow - x.UploadedAt < TimeSpan.FromMinutes(1))
                .ToList();
            var uploadedFilesLastHour = uploadedFiles
                .Where(x => DateTime.UtcNow - x.UploadedAt < TimeSpan.FromHours(1))
                .ToList();
            var uploadedFilesLastDay = uploadedFiles
                .Where(x => DateTime.UtcNow - x.UploadedAt < TimeSpan.FromDays(1))
                .ToList();
            long sumSizeLastMinute = uploadedFilesLastMinute.Sum(x => x.Size);
            long sumSizeLastHour = uploadedFilesLastHour.Sum(x => x.Size);
            long sumSizeLastDay = uploadedFilesLastDay.Sum(x => x.Size);
            //50MB limit per minute
            //150MB limit per hour
            //300MB limit per day
            if (sumSizeLastMinute > 50 * 1024 * 1024 || sumSizeLastHour > 150 * 1024 * 1024 || sumSizeLastDay > 300 * 1024 * 1024) return true;
            return false;
        }

        private async Task<bool> IsStorageFull()
        {
            long maxStorageSize = (long)_configuration.GetValue<int>("MaxStorageSize") * 1024 * 1024 * 1024;
            long storageUsed = await _databaseContext.UploadedFiles.SumAsync(x => x.Size);
            return storageUsed > maxStorageSize;
        }

        private async Task<string> UploadAsync(UploadForm uploadForm)
        {
            string ip = HttpContext.Request.Headers["X-Forwarded-For"];

            string randomId = RandomHelper.GenerateRandomString(5);
            string serverName = Environment.MachineName;
            string hostname = HttpContext.Request.Host.Value;

            string originalFilename;
            if (uploadForm.Files.Count == 1)
                originalFilename = SanitizeFileName(uploadForm.Files.First().FileName);
            else
                originalFilename = "yourfiles.zip";

            string filename = randomId + "-" + originalFilename;
            var uploadedAt = DateTime.UtcNow;
            string coupon = uploadForm.Code;
            int lifetime = uploadForm.Lifetime;

            long fileSize = uploadForm.Files.Sum(x => x.Length);

            string uploadsPath = _configuration.GetValue<string>("UploadsPath");
            CreateDirectoryIfNotExists(uploadsPath);
            string mainUploadPath = Path.Combine(uploadsPath, "Main");
            CreateDirectoryIfNotExists(mainUploadPath);
            string tempUploadPath = Path.Combine(uploadsPath, "Temp");
            CreateDirectoryIfNotExists(tempUploadPath);
            string fullSavePath = Path.Combine(mainUploadPath, filename);

            if (uploadForm.Files.Count == 1)
            {
                using var stream = new FileStream(fullSavePath, FileMode.Create);
                await uploadForm.Files.First().CopyToAsync(stream);
            }
            else
            {
                string tempFolder = Path.Combine(tempUploadPath, randomId);
                CreateDirectoryIfNotExists(tempFolder);
                foreach (var file in uploadForm.Files)
                {
                    string safeFilename = SanitizeFileName(file.FileName);
                    string savePath = Path.Combine(tempFolder, safeFilename);
                    using var stream = new FileStream(savePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
                using var zip = new ZipFile();
                zip.AddDirectory(tempFolder);
                zip.Save(fullSavePath);
                Directory.Delete(tempFolder, true);
            }

            var uploadedFile = new UploadedFile
            {
                RandomId = randomId,
                ServerName = serverName,
                Hostname = hostname,
                Filename = originalFilename,
                UploadedAt = uploadedAt,
                Lifetime = lifetime,
                Ip = ip,
                Size = fileSize,
                Coupon = coupon
            };
            _databaseContext.UploadedFiles.Add(uploadedFile);
            await _databaseContext.SaveChangesAsync();

            return randomId;
        }

        private static void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        private static string SanitizeFileName(string name)
        {
            var invalids = Path.GetInvalidFileNameChars();
            return string.Join("_", name.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
        }
    }
}
