using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDownl_Shared_Resources.Models
{
    public class Folder
    {
        public string Name { get; set; }
        public List<Folder> Folders { get; set; } = new List<Folder>();
        public List<string> Files { get; set; } = new List<string>();
    }
}
