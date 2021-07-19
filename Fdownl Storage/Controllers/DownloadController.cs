using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fdownl_Storage.Models;
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

        public IActionResult Index(string fileName)
        {
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string mainUploadPath = Path.Combine(contentRootPath, "Uploads", "Main");
            string filePath = Path.Combine(mainUploadPath, fileName);

            string originalFileName = fileName.Substring(fileName.IndexOf('-') + 1);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            string contentType;
            if(!(new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType)))
            {
                contentType = "application/octet-stream";
            }

            return PhysicalFile(filePath, contentType, originalFileName, true);
        }
    }
}