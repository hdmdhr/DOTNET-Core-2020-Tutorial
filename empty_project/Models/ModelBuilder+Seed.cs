using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace empty_project.Models
{
    public static class ModelBuilderExt
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "Mark Jensen", Email = "mark@sait.com", Department = Departments.HR },
            new Employee { Id = 2, Name = "Jason Swift", Email = "jason@vog.com", Department = Departments.Payroll });
        }
    }
}
