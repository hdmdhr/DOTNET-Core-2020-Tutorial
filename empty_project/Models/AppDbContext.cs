using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace empty_project.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        // for every custom class in the app, need one property
        // LINQ query against DbSet<TEntity> will be translated into SQL query
        public DbSet<Employee> Employees { get; set; }
    }
}
