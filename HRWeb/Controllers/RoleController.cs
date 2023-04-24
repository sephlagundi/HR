﻿using HRWeb.Models;
using HRWeb.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRWeb.Controllers
{
    public class RoleController : Controller
    {
        // Used with model to work with
        // IdentityRole in Identity Framework

        public RoleManager<IdentityRole> _roleManager { get; }
        /*private UserManager<ApplicationUser> _userManager { get; }*/

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            /*_userManager = userManager;*/

        }

        [HttpGet]
        public IActionResult Create()
        {
            // _roleManager.CreateAsync();
            // _roleManager.Roles;
            // _roleManager.DeleteAsync();
            // _roleManager.UpdateAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = roleViewModel.Name
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    RedirectToAction("GetAllRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

            return View(roleViewModel);
        }



        [HttpGet]
        public IActionResult GetAllRoles()
        {

            return View(_roleManager.Roles.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Update(string roleId)
        {
            var oldRole = await _roleManager.FindByIdAsync(roleId);
            return View(oldRole);
        }
        [HttpPost]
        public async Task<IActionResult> Update(RoleViewModel role)
        {
            var oldRole = await _roleManager.FindByIdAsync(role.Id.ToString());
            oldRole.Name = role.Name;
            var result = await _roleManager.UpdateAsync(oldRole);
            if (result.Succeeded)
            {
                return RedirectToAction("GetAllRoles");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        public IActionResult Details(string roleId)
        {
            var role = _roleManager.FindByIdAsync(roleId);
            return View(role.Result);
        }

        public async Task<IActionResult> Delete(string roleId)
        {
            var oldRole = await _roleManager.FindByIdAsync(roleId);

            var rolelist = _roleManager.DeleteAsync(oldRole);
            return RedirectToAction(controllerName: "Role", actionName: "GetAllRoles"); // reload the getall page it self
        }



    }
}
