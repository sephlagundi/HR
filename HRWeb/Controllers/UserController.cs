﻿using HRWeb.Data;
using HRWeb.Models;
using HRWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HRWeb.Controllers
{

    /*[Authorize(Roles = "Administrator")]*/
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private UserManager<ApplicationUser> _userManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }
        public SignInManager<ApplicationUser> _signInManager { get; }
        public UserController(UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }
        // [AllowAnonymous]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var userViewModel = new List<UserRoleViewModel>();
            var userWithRole = _roleManager.Roles.ToList();

            foreach (var role in userWithRole)
            {
                var userlist = await _userManager.GetUsersInRoleAsync(role.Name);
                foreach (var user in userlist)
                {
                    // Get department name from department ID
                    var departmentName = string.Empty;
                    if (user.DepartmentId != null)
                    {
                        var department = await _dbContext.Departments.FindAsync(user.DepartmentId);
                        if (department != null)
                        {
                            departmentName = department.Name;
                        }
                    }

                    userViewModel.Add(new UserRoleViewModel
                    {
                        userId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        roleName = role.Name,
                        DepartmentId = user.DepartmentId,
                        DepartmentName = departmentName
                    });
                }
            }

            return View(userViewModel);
        }





        public IActionResult Details(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            return View(user);
        }
        public async Task<IActionResult> Delete(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            var userlist = await _userManager.DeleteAsync(user);
            return RedirectToAction(controllerName: "User", actionName: "GetAllUsers"); // reload the getall page it self
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterUserViewModel userViewModel) // model binded this where the views data is accepted 
        {
            if (ModelState.IsValid)
            {
                var userModel = new ApplicationUser
                {
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName
                };
                var result = await _userManager.CreateAsync(userModel, userViewModel.Password);
                if (result.Succeeded)
                {
                    //ADD ROLES AND ALLOW THEM TO LOGIN
                    // ASSIGN DEFAULT ROLE 
                    var role = _roleManager.Roles.FirstOrDefault(r => r.Name == "User");
                    if (role != null)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(userModel, role.Name);

                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError(String.Empty, "Role cannot be assigned");
                        }
                    }



                    //LOG IN THE USER AUTOMATICALLY
                    await _signInManager.SignInAsync(userModel, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

            return View(userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            var roles = await _userManager.GetRolesAsync(user);
            EditUserViewModel userViewModel = new EditUserViewModel()
            {
                FirstName = user.FirstName,
                Email = user.Email,
                LastName = user.LastName,
                Roles = roles
            };
            return View(userViewModel);
        }
        [HttpPost]
        public IActionResult Update(EditUserViewModel user)
        {
            //var user = _userManager.Users.FirstOrDefault(u => u.Id == newUser);

            return RedirectToAction("GetAllUsers");
        }
    }
}
