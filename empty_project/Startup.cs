using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using empty_project.Models;
using empty_project.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
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
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequiredUniqueChars = 3;
                })
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy", 
                    policy => policy.RequireClaim("Delete Role")
                        .RequireClaim("Create Role"));
                // Custom Policy: to edit role, user either needs to be SuperAdmin, or CompanyAdmin with Edit Role claim
                options.AddPolicy("EditRolePolicy",
                    policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
            });

            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;

                    // enable pocily globally
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddXmlSerializerFormatters();  // this enable content negotiation for XML (Accept: application/xml)

            // customize AccessDenied route
            services.ConfigureApplicationCookie(options => options.AccessDeniedPath = new PathString("/Administration/AccessDenied"));

            services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();

            services.AddSingleton<IAuthorizationHandler, CanOnlyEditOtherAdminRolesAndClaimsHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();  // this MW should be plugged in ASAP
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseStaticFiles();  // Order Reason: if request is for static files, this MW can short circuit to avoid extra processing

            app.UseAuthentication();

            //app.UseMvcWithDefaultRoute();  // this is for .NET Core 2.2
            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });  // manually apply default route

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(env.EnvironmentName);
                });
            });
        }
    }
}
