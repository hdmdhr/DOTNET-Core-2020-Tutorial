using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using empty_project.Models;
using empty_project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace empty_project.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(vm.RoleName);
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("ListRoles");

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id: {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel{Id = id, RoleName = role.Name};

            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                    model.Users.Add(user.UserName);
            }

            return View(model);

         
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel vm)
        {
            var role = await _roleManager.FindByIdAsync(vm.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id: {vm.Id} cannot be found";
                return View("NotFound");
            }

            role.Name = vm.RoleName;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
                return RedirectToAction("ListRoles");

            foreach(var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(vm);
        }
    }
}
