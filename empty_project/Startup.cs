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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // create options to config Developer Exception Page
                var devExceptionPageOptions = new DeveloperExceptionPageOptions
                {
                    SourceCodeLineCount = 10  // line numbers to display before & after exception throwing line
                };
                app.UseDeveloperExceptionPage(devExceptionPageOptions);  // this MW should be plugged in ASAP
            }

            // create & config file server options
            var fileServerOptions = new FileServerOptions();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("foo.html");

            app.UseFileServer(fileServerOptions);  // 2 combined in 1

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    throw new Exception("Something went wrong processing the request!");
                    await context.Response.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName + _config["MyKey"]);
                });
            });
        }
    }
}
