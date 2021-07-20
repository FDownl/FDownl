using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FDownl_Shared_Resources.Models;

namespace FDownl_Shared_Resources
{
    public class DatabaseContext : DbContext
    {
        public DbSet<StorageServer> StorageServers { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }
    }
}
