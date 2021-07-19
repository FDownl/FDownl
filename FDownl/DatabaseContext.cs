using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FDownl.Models;

namespace FDownl
{
    public class DatabaseContext : DbContext
    {
        public DbSet<StorageServer> StorageServers { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }
    }
}
