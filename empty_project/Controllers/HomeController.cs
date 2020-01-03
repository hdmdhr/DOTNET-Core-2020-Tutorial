using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using empty_project.Models;
using empty_project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace empty_project.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;

        public HomeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        [Route("~/Home")]
        [Route("~/")]
        public IActionResult Index()
        {
            var model = _employeeRepo.GetAllEmployees();
            return View(model);
            //return Json(_employeeRepo.GetEmployee(1));
        }

        [Route("{id?}")]
        public IActionResult Details(int? id)
        {
            var homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepo.GetEmployee(id ?? 1),
                PageTitle = "Employee Details"
            };
            return View(homeDetailsViewModel);
        }
    }
}