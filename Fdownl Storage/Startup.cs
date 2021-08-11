using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FDownl_Shared_Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Quartz;

namespace Fdownl_Storage
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetValue<string>("ConnectionString");

            services.AddDbContext<DatabaseContext>(
                options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );

            services.AddQuartz(x =>
            {
                x.UseMicrosoftDependencyInjectionScopedJobFactory();
                x.ScheduleJob<FileDeletionJob>(trigger => trigger
                .WithIdentity("file_deletion_trigger")
                .WithSimpleSchedule(x => x.RepeatForever().WithIntervalInMinutes(1)));
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            services.AddCors();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions { 
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto 
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();

            app.UseCors(x => x.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
