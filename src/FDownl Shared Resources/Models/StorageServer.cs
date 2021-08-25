using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FDownl_Shared_Resources.Models
{
    public class StorageServer
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Hostname { get; set; }
        public string Location { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
