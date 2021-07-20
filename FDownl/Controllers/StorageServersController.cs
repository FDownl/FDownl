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
    public class StorageServersController : Controller
    {
        private readonly ILogger<StorageServersController> _logger;
        private readonly DatabaseContext _databaseContext;

        public StorageServersController(ILogger<StorageServersController> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task<IActionResult> GetAsync()
        {
            var storageServers = await _databaseContext.StorageServers.ToListAsync();
            return Json(storageServers);
        }
    }
}
