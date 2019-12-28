using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace empty_project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Json(new {id=1, name="Daniel Hu"});
        }
    }
}