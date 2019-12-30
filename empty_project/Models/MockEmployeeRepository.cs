using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace empty_project.Models
{
    public class MockEmployeeRepository: IEmployeeRepository
    {
        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee(){Id = 1, Department = "Sales", Email = "1@2.com", Name = "Jeff Fired"},
                new Employee(){Id = 2, Department = "Admin", Email = "1@3.com", Name = "Vince Walkman"},
                new Employee(){Id = 3, Department = "IT", Email = "1@4.com", Name = "Daniel Bravo"}
            };
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }
    }
}
