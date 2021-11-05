using FDownl_Shared_Resources;
using FDownl_Shared_Resources.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

namespace FDownl.Pages
{
    public class ShareModel : PageModel
    {
        private readonly ILogger<ShareModel> _logger;
        private readonly DatabaseContext _context;
        private readonly int range = 1000;

        public UploadedFile UploadedFile { get; set; }
        public string FileContent = null;

        public ShareModel(ILogger<ShareModel> logger, DatabaseContext context) {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string id) {
            UploadedFile = await _context.UploadedFiles.FirstOrDefaultAsync(x => x.RandomId == id);
            if (UploadedFile != null && UploadedFile.UploadedAt.AddSeconds(UploadedFile.Lifetime) < DateTime.UtcNow)
                UploadedFile = null;

            if (UploadedFile == null)
                return NotFound();
            else
            {
                FileContent = HasBinaryContent(UploadedFile);
                return Page();
            }
        }

        public string HasBinaryContent(UploadedFile file)
        {
            string results = "";
            char[] c = new char [range];
            string url = "https://" + file.Hostname + file.RandomId + file.Filename;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            sr.Read(c, 0, c.Length > (int)resp.ContentLength ? c.Length : (int)resp.ContentLength);
            sr.Close();
            results = new string(c);
            // if (results.Any(ch => char.IsControl(ch) && ch != '\r' && ch != '\n' && ch != '\t'))
            //     return null;
            return results;
        }
    }
}
