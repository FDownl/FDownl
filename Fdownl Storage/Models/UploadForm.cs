using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FDownl.Models
{
    public class UploadForm
    {
        [Required]
        public List<IFormFile> Files { get; set; }
        [Required]
        [Range(60,604800)]
        public int Lifetime { get; set; }
        public string Code { get; set; }
    }
}
