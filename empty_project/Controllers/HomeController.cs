using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using empty_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace empty_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;

        public HomeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public IActionResult Index()
        {
            return Json(_employeeRepo.GetEmployee(1));
        }

        public IActionResult Details()
        {
            var model = _employeeRepo.GetEmployee(2);
            ViewBag.Employee = model;  // ViewBag use dynamic property, type is decided at Run Time, no need to do casting in View
            ViewBag.PageTitle = "Employee Details";
            return View();
        }
    }
}