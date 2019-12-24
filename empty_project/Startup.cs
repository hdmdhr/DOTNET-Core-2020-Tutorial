using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace empty_project
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.Use(async (HttpContext context, Func<Task> next) =>
            {
                logger.LogInformation("MW1: Incoming request to Middleware 1st.");
                await next();
                logger.LogInformation("MW1: Outgoing response from 1st MW");
            });

            app.Use(async (HttpContext context, Func<Task> next) =>
            {
                logger.LogInformation("MW2: Incoming request to MW 2nd.");
                await next();
                logger.LogInformation("MW2: Outgoing response from 2nd MW");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName + _config["MyKey"]);
                });
            });
        }
    }
}
