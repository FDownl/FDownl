using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FDownl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FDownl.Controllers
{
    public class ShareController : Controller
    {
        private readonly ILogger<ShareController> _logger;
        private readonly DatabaseContext _databaseContext;

        public ShareController(ILogger<ShareController> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public IActionResult Index(string id)
        {
            UploadedFile file = null;
            if (ModelState.IsValid)
            {
                file = _databaseContext.UploadedFiles.Where(x => x.RandomId == id).FirstOrDefault();
            }
            return View(file);
        }
    }
}
