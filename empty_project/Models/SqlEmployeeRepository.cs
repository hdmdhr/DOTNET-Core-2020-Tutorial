using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace empty_project.Models
{
    public class SqlEmployeeRepository: IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public SqlEmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public Employee GetEmployee(int id)
        {
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
            throw new NotImplementedException();
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
