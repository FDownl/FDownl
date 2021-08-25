using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDownl_Shared_Resources.Models
{
    public class CouponCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int LifetimeAdd { get; set; }
        public int LifetimeSet { get; set; }
    }
}
