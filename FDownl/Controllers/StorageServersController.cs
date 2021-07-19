using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FDownl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FDownl.Controllers
{
    [Route("api/[controller]/[action]")]
    public class StorageServersController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _databaseContext;

        public StorageServersController(ILogger<HomeController> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task<IActionResult> Get()
        {
            List<StorageServer> storageServers = await _databaseContext.StorageServers.ToListAsync();
            return Json(storageServers);
        }
    }
}
