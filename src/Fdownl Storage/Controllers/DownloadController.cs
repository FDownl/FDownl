using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FDownl_Shared_Resources;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fdownl_Storage.Controllers
{
    public class DownloadController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DownloadController> _logger;
        private readonly DatabaseContext _databaseContext;

        public DownloadController(IConfiguration configuration,ILogger<DownloadController> logger, DatabaseContext databaseContext)
        {
            _configuration = configuration;
            _logger = logger;
            _databaseContext = databaseContext;
        }

        [Route("/{filename}")]
        [EnableCors]
        public IActionResult Index(string fileName)
        {
            string uploadsPath = _configuration.GetValue<string>("UploadsPath");
            string filePath = Path.Combine(uploadsPath, "Main", fileName);

            if (!System.IO.File.Exists(filePath)) return NotFound();

            if (!new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType))
                contentType = "application/octet-stream";

            string originalFileName = fileName[(fileName.IndexOf('-') + 1)..];

            return PhysicalFile(filePath, contentType, originalFileName, true);
        }
    }
}