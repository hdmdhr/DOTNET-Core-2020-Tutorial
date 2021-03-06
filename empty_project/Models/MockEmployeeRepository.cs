﻿using System;
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
                new Employee(){Id = 1, Department = Departments.HR, Email = "1@2.com", Name = "John Doe"},
                new Employee(){Id = 2, Department = Departments.Payroll, Email = "1@3.com", Name = "Sony Walkman"},
                new Employee(){Id = 3, Department = Departments.IT, Email = "1@4.com", Name = "Donald Bravo"}
            };
        }

        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Update(Employee changedEmployee)
        {
            var employee2Update = _employeeList.FirstOrDefault(e => e.Id == changedEmployee.Id);
            if (employee2Update != null)
            {
                employee2Update = changedEmployee;
            }

            return employee2Update;
        }

        public Employee Delete(int id)
        {
            var employee2Delete = _employeeList.FirstOrDefault(e => e.Id == id);
            if (employee2Delete != null)
            {
                _employeeList.Remove(employee2Delete);
            }

            return employee2Delete;
        }
    }
}
