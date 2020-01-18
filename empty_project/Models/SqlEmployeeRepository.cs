using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace empty_project.Models
{
    public class SqlEmployeeRepository: IEmployeeRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SqlEmployeeRepository> _logger;

        public SqlEmployeeRepository(AppDbContext context, ILogger<SqlEmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Employee GetEmployee(int id)
        {
            _logger.LogTrace("Trace Log");
            _logger.LogDebug("Debug Log");
            _logger.LogInformation("Information Log");
            _logger.LogWarning("Warning Log");
            _logger.LogError("Error Log");
            _logger.LogCritical("Critical Log");

            return _context.Employees.Find(id);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees;
        }

        public Employee Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public Employee Update(Employee changedEmployee)
        {
            var employee = _context.Employees.Attach(changedEmployee);
            employee.State = EntityState.Modified;
            _context.SaveChanges();
            return changedEmployee;
        }

        public Employee Delete(int id)
        {
            var employee2Delete = _context.Employees.Find(id);
            if (employee2Delete != null)
            {
                _context.Employees.Remove(employee2Delete);
            }

            return employee2Delete;
        }
    }
}
