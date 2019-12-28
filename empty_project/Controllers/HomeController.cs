﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using empty_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace empty_project.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepo;

        public HomeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public IActionResult Index()
        {
            return Json(_employeeRepo.GetEmployee(1));
        }
    }
}