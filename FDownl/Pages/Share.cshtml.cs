using FDownl_Shared_Resources.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDownl.Pages
{
    public class ShareModel : PageModel
    {
        private readonly ILogger<ShareModel> _logger;
        private readonly DatabaseContext _context;

        public UploadedFile UploadedFile { get; set; };

        public ShareModel(ILogger<ShareModel> logger, DatabaseContext context) {
            _logger = logger;
            _context = context;
        }

        public async Task OnGetAsync(string id) {
            UploadedFile = await _context.UploadedFiles.Where(x => x.RandomId == id).FirstOrDefaultAsync();
        }
    }
}
