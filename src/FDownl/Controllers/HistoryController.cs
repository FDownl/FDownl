using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FDownl_Shared_Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FDownl.Controllers
{
    [Route("api/[controller]/[action]")]
    public class HistoryController : Controller
    {
        private readonly ILogger<HistoryController> _logger;
        private readonly DatabaseContext _databaseContext;

        public HistoryController(ILogger<HistoryController> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }
        
        public async Task<IActionResult> GetAsync()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            var history = await _databaseContext.UploadedFiles
                .Where(x => x.Ip == ip && x.IsPublic)
                .Select(x => new ApiFile { Id = x.Id, RandomId = x.RandomId, Hostname = x.Hostname, Filename = x.Filename, UploadedAt = x.UploadedAt, Lifetime = x.Lifetime, Size = x.Size } )
                .ToListAsync();

            return Json(history);
        }

        public class ApiFile
        {
            public int Id { get; set; }
            public string RandomId { get; set; }
            public string Hostname { get; set; }
            public string Filename { get; set; }
            public DateTime UploadedAt { get; set; }
            public int Lifetime { get; set; }
            public long Size { get; set; }
        }
    }
}
