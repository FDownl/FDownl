using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FDownl_Shared_Resources;
using Fdownl_Storage.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace Fdownl_Storage.Controllers
{
    public class DownloadController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<DownloadController> _logger;
        private readonly DatabaseContext _databaseContext;

        public DownloadController(IWebHostEnvironment webHostEnvironment, ILogger<DownloadController> logger, DatabaseContext databaseContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _databaseContext = databaseContext;
        }

        [Route("/{filename}")]
        [EnableCors]
        public IActionResult Index(string fileName)
        {
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string mainUploadPath = Path.Combine(contentRootPath, "Uploads", "Main");
            string filePath = Path.Combine(mainUploadPath, fileName);

            string originalFileName = fileName[(fileName.IndexOf('-') + 1)..];

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            if (!(new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType)))
            {
                contentType = "application/octet-stream";
            }

            return PhysicalFile(filePath, contentType, originalFileName, true);
        }
    }
}