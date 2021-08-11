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
    public class CouponsController : Controller
    {
        private readonly ILogger<StorageServersController> _logger;
        private readonly DatabaseContext _databaseContext;

        public CouponsController(ILogger<StorageServersController> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task<IActionResult> ByCodeAsync()
        {
            var coupon = await _databaseContext.CouponCodes
                .Select(x => new ApiCoupon { Id = x.Id, Code = x.Code, LifetimeAdd = x.LifetimeAdd, LifetimeSet = x.LifetimeSet } )
                .FirstOrDefaultAsync();
            return Json(coupon);
        }

        public class ApiCoupon
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public int LifetimeAdd { get; set; }
            public int LifetimeSet { get; set; }
        }
    }
}
