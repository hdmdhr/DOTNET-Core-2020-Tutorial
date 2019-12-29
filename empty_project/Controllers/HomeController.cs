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
            return View("Views/Home/Test.cshtml");  // 4 overloads: no param; model; file name (without extension); absolute (with ext) / relative (without ext) path
            return new ObjectResult(_employeeRepo.GetEmployee(2)); 
        }
    }
}