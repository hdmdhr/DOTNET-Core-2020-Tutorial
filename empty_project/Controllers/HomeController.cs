using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using empty_project.Models;
using empty_project.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace empty_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepo, IHostingEnvironment hostingEnvironment)
        {
            _employeeRepo = employeeRepo;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var model = _employeeRepo.GetAllEmployees();
            return View(model);
            //return Json(_employeeRepo.GetEmployee(1));
        }

        
        public IActionResult Details(int? id)
        {
            var homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepo.GetEmployee(id ?? 1),
                PageTitle = "Employee Details"
            };
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (vm.Photo != null)
                {
                    // generate unique file path
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = $"{Guid.NewGuid().ToString()}_{vm.Photo.FileName}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    // copy uploaded photo to specified file path
                    vm.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                var newEmployee = _employeeRepo.Add(new Employee
                {
                    Name = vm.Name,
                    Email = vm.Email,
                    Department = vm.Department,
                    PhotoPath = uniqueFileName
                });
                return RedirectToAction("Details", new {id = newEmployee.Id});
            }

            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _employeeRepo.GetEmployee(id);
            var vm = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath,
                Photo = null
            };
            return View(vm);
        }
    }
}