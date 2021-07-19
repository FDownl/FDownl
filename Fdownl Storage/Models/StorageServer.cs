using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fdownl_Storage.Models
{
    public class StorageServer
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Hostname { get; set; }
        public string Location { get; set; }
        public bool IsFull { get; set; }
    }
}
