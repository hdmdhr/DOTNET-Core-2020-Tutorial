using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace empty_project.Models
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>  // adding <class> is important for migration
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        // for every custom class in the app, need one property
        // LINQ query against DbSet<TEntity> will be translated into SQL query
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // to prevent Add-Migration error `entity type 'IdentityUserLogin<string>' requires a primary key to be defined`, call OnModelCreating() on base class
            base.OnModelCreating(modelBuilder);  // calling this will have PK mapped

            modelBuilder.Seed();

            // change OnDelete from default Cascade to NoAction
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
