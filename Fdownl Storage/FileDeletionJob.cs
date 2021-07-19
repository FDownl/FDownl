using Fdownl_Storage.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Fdownl_Storage
{
    public class FileDeletionJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FileDeletionJob> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileDeletionJob(IServiceProvider serviceProvider, ILogger<FileDeletionJob> logger, IWebHostEnvironment webHostEnvironment)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public Task Execute(IJobExecutionContext context)
        {
            string serverName = Environment.MachineName;

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                _logger.LogInformation("Running file deletion schedule");
                using (DatabaseContext databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
                {
                    List<UploadedFile> filesToDelete = databaseContext.UploadedFiles
                        .Where(x => x.ServerName == serverName
                            && x.UploadedAt.AddSeconds(x.Lifetime) < DateTime.UtcNow)
                        .ToList();
                    foreach (UploadedFile file in filesToDelete)
                    {
                        string contentRootPath = _webHostEnvironment.ContentRootPath;
                        string mainUploadPath = Path.Combine(contentRootPath, "Uploads", "Main");
                        if (Directory.Exists(mainUploadPath))
                        {
                            string filePath = Path.Combine(mainUploadPath, file.RandomId + "-" + file.Filename);
                            databaseContext.UploadedFiles.Remove(file);
                            databaseContext.SaveChanges();
                            File.Delete(filePath);
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
