﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using empty_project.Models;
using empty_project.Utilities;
using empty_project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace empty_project.Controllers
{
    [Authorize(Roles = "Company Admin, Super Admin")]
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
        [Authorize(Roles = "Super Admin")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
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
        public IActionResult ListUsers()
        {
            var users = _userManager.Users.ToList();
            return View(users);
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

            var model = new EditRoleViewModel { Id = id, RoleName = role.Name };

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

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id: {role} cannot be found.";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users.ToList())
            {
                var userRoleVM = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                userRoleVM.IsSelected = await _userManager.IsInRoleAsync(user, role.Name);
                model.Add(userRoleVM);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> viewModels, string roleId)  // roleId is carried here by query string
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id: {roleId} cannot be found.";
                return View("NotFound");
            }

            foreach (var (vm, i) in viewModels.WithIndex())
            {
                var user = await _userManager.FindByIdAsync(vm.UserId);
                IdentityResult result = null;

                if (vm.IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                else if (!vm.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                else
                    continue;

                if (result.Succeeded)
                {
                    if (i < viewModels.Count - 1)  // more to loop
                        continue;

                    return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id: {id} cannot be found.";
                return View("NotFound");
            }

            var vm = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                Claims = (await _userManager.GetClaimsAsync(user)).Select(c => c.Value).ToList(),
                Roles = (await _userManager.GetRolesAsync(user))
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id: {model.Id} cannot be found.";
                return View("NotFound");
            }

            user.Email = model.Email;
            user.UserName = model.UserName;
            user.City = model.City;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToAction("ListUsers");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id: {id} cannot be found.";
                return View("NotFound");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return RedirectToAction("ListUsers");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View("ListUsers");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id: {id} cannot be found.";
                return View("NotFound");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
                return RedirectToAction("ListRoles");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View("ListRoles");
        }
    }
}