using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FDownl_Shared_Resources.Models
{
    public class UploadedFile
    {
        public int Id { get; set; }
        public string RandomId { get; set; }
        public string ServerName { get; set; }
        public string Hostname { get; set; }
        public string Filename { get; set; }
        public DateTime UploadedAt { get; set; }
        public int Lifetime { get; set; }
        public string Ip { get; set; }
        public long Size { get; set; }
        public string Coupon { get; set; }
        public bool IsEncrypted { get; set; }
        public string ZipContents { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
