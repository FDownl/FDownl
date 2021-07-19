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
    public class FaqController : Controller
    {
        private readonly ILogger<FaqController> _logger;
        private readonly DatabaseContext _databaseContext;

        public FaqController(ILogger<FaqController> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
