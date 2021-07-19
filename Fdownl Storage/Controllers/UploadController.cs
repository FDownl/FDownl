using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FDownl.Models;
using Fdownl_Storage.Models;
using Ionic.Zip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fdownl_Storage.Controllers
{
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<UploadController> _logger;
        private readonly DatabaseContext _databaseContext;

        public UploadController(IWebHostEnvironment webHostEnvironment, ILogger<UploadController> logger, DatabaseContext databaseContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _databaseContext = databaseContext;
        }

        [HttpPost]
        [RequestSizeLimit(52428800)] // 50MB = 50 * 1024 * 1024
        public async Task<IActionResult> Index(UploadForm uploadForm)
        {
            UploadResult result = new UploadResult()
            {
                Code = 0,
                Id = null,
                Error = null
            };
            if (ModelState.IsValid)
            {
                if (ClientValidation())
                {
                    if (!IsDiskFull())
                    {
                        result.Id = await UploadAsync(uploadForm);
                    }
                    else
                    {
                        await SetServerFullState(true);
                        result.Code = 3;
                        result.Error = "The server you specified is full. Please choose another.";
                    }
                }
                else
                {
                    result.Code = 2;
                    result.Error = "You are uploading too many files too fast. Please try again later.";
                }
            }
            else
            {
                result.Code = 1;
                result.Error = "Invalid options supplied!";
            }
            return Json(result);
        }

        private bool ClientValidation()
        {
            string ip = HttpContext.Request.Headers["X-Forwarded-For"];
            List<UploadedFile> uploadedFiles = _databaseContext.UploadedFiles
                .Where(x => x.Ip == ip)
                .ToList();
            List<UploadedFile> uploadedFilesLastMinute = uploadedFiles
                .Where(x => DateTime.UtcNow - x.UploadedAt < TimeSpan.FromMinutes(1))
                .ToList();
            List<UploadedFile> uploadedFilesLastHour = uploadedFiles
                .Where(x => DateTime.UtcNow - x.UploadedAt < TimeSpan.FromHours(1))
                .ToList();
            List<UploadedFile> uploadedFilesLastDay = uploadedFiles
                .Where(x => DateTime.UtcNow - x.UploadedAt < TimeSpan.FromDays(1))
                .ToList();
            long sumSizeLastMinute = 0;
            long sumSizeLastHour = 0;
            long sumSizeLastDay = 0;
            foreach (UploadedFile f in uploadedFilesLastMinute) { sumSizeLastMinute += f.Size; }
            foreach (UploadedFile f in uploadedFilesLastHour) { sumSizeLastHour += f.Size; }
            foreach (UploadedFile f in uploadedFilesLastDay) { sumSizeLastDay += f.Size; }
            //50MB limit per minute
            if (sumSizeLastMinute > 50 * 1024 * 1024) { return false; }
            //150MB limit per hour
            if (sumSizeLastHour > 150 * 1024 * 1024) { return false; }
            //300MB limit per day
            if (sumSizeLastDay > 300 * 1024 * 1024) { return false; }

            return true;
        }

        private async Task<string> UploadAsync(UploadForm uploadForm)
        {
            UploadedFile uploadedFile = new UploadedFile();

            string randomIdentifier = GenerateRandomIdentifier();

            string serverName = Environment.MachineName;

            string hostname = HttpContext.Request.Host.Value;

            string originalFilename = "";
            if (uploadForm.Files.Count == 1)
            {
                IFormFile file = uploadForm.Files.First();
                originalFilename = SanitizeFileName(file.FileName);
            }
            else
            {
                originalFilename = "yourfiles.zip";
            }
            string filename = randomIdentifier + "-" + originalFilename;

            DateTime uploadedAt = DateTime.UtcNow;

            string coupon = uploadForm.Code;

            switch (coupon)
            {
                case "90DAYSOFLIFETIMEFORCTFSERVER":
                    uploadForm.Lifetime = 90 * 24 * 60 * 60; //90 days
                    break;
                case "8WEEKSOFLIFETIMEFROMCTFSERVERGIVEAWAY":
                    uploadForm.Lifetime = 8 * 7 * 24 * 60 * 60; //8 weeks
                    break;
            }

            int lifetime = uploadForm.Lifetime;

            string ip = HttpContext.Request.Headers["X-Forwarded-For"];

            long fileSize = 0;
            foreach (IFormFile file in uploadForm.Files)
            {
                fileSize += file.Length;
            }

            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string uploadPath = Path.Combine(contentRootPath, "Uploads");
            CreateDirectoryIfNotExists(uploadPath);
            string mainUploadPath = Path.Combine(contentRootPath, "Uploads", "Main");
            CreateDirectoryIfNotExists(mainUploadPath);
            string tempUploadPath = Path.Combine(contentRootPath, "Uploads", "Temp");
            CreateDirectoryIfNotExists(tempUploadPath);
            string fullSavePath = Path.Combine(mainUploadPath, filename);

            if (uploadForm.Files.Count == 1)
            {
                IFormFile file = uploadForm.Files.First();
                using (FileStream stream = new FileStream(fullSavePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            else
            {
                string tempFolder = Path.Combine(tempUploadPath, randomIdentifier);
                CreateDirectoryIfNotExists(tempFolder);
                foreach (IFormFile file in uploadForm.Files)
                {
                    string safeFilename = SanitizeFileName(file.FileName);
                    string savePath = Path.Combine(tempFolder, safeFilename);
                    using (FileStream stream = new FileStream(savePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(tempFolder);
                    zip.Save(fullSavePath);
                }
                Directory.Delete(tempFolder, true);
            }

            uploadedFile.RandomId = randomIdentifier;
            uploadedFile.ServerName = serverName;
            uploadedFile.Hostname = hostname;
            uploadedFile.Filename = originalFilename;
            uploadedFile.UploadedAt = uploadedAt;
            uploadedFile.Lifetime = lifetime;
            uploadedFile.Ip = ip;
            uploadedFile.Size = fileSize;
            uploadedFile.Coupon = coupon;
            _databaseContext.UploadedFiles.Add(uploadedFile);
            await _databaseContext.SaveChangesAsync();

            return randomIdentifier;
        }

        private async Task SetServerFullState(bool isFull)
        {
            string hostname = HttpContext.Request.Host.Value;
            StorageServer server = _databaseContext.StorageServers.FirstOrDefault(item => item.Hostname == hostname);
            server.IsFull = isFull;
            await _databaseContext.SaveChangesAsync();
        }

        private bool IsDiskFull()
        {
            long freeSpace = 0;
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    freeSpace += drive.AvailableFreeSpace;
                }
            }
            if (freeSpace >= 50 * 1024 * 1024)
            {
                return false;
            }
            return true;
        }

        private void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private string GenerateRandomIdentifier()
        {
            Random random = new Random();
            byte[] randomBytes = new byte[5];
            random.NextBytes(randomBytes);
            string randomHex = BitConverter.ToString(randomBytes).Replace("-", string.Empty);
            return randomHex;
        }

        private string SanitizeFileName(string name)
        {
            var invalids = Path.GetInvalidFileNameChars();
            var newName = string.Join("_", name.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
            return newName;
        }
    }
}
