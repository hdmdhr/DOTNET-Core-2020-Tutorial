using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using empty_project.Models;
using empty_project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace empty_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IHostingEnvironment _hostingEnvironment;
        private string _imagesFolderPath;

        public HomeController(IEmployeeRepository employeeRepo, IHostingEnvironment hostingEnvironment)
        {
            _employeeRepo = employeeRepo;
            _hostingEnvironment = hostingEnvironment;
            _imagesFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "images");
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = _employeeRepo.GetAllEmployees();
            return View(model);
            //return Json(_employeeRepo.GetEmployee(1));
        }

        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            var employee = _employeeRepo.GetEmployee(id.Value);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            var homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
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
                var uniqueFileName = CopyToUniquePath(vm.Photo);
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

        private string CopyToUniquePath(IFormFile photo)
        {
            string uniqueFileName = null;
            if (photo != null)
            {
                // generate unique file path
                uniqueFileName = $"{Guid.NewGuid().ToString()}_{photo.FileName}";
                string filePath = Path.Combine(_imagesFolderPath, uniqueFileName);
                // copy uploaded photo to specified file path
                using var fileStream = new FileStream(filePath, FileMode.Create);
                photo.CopyTo(fileStream);
            }

            return uniqueFileName;
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

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var employee = _employeeRepo.GetEmployee(vm.Id);
                employee.Name = vm.Name;
                employee.Email = vm.Email;
                employee.Department = vm.Department;

                if (vm.Photo != null)
                {
                    if (vm.ExistingPhotoPath != null)  // user has old photo
                    {
                        var filePath = Path.Combine(_imagesFolderPath, vm.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = CopyToUniquePath(vm.Photo);
                }

                _employeeRepo.Update(employee);
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}