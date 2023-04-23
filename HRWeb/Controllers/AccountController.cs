using HRWeb.Data;
using HRWeb.Models;
using HRWeb.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;

namespace HRWeb.Controllers
{
    public class AccountController : Controller
    {
        // Configures REG PAGE
        private readonly ApplicationDbContext _dbContext;

        //MANAGE USER ACTIVITIES LIKE CRUD  
        public UserManager<ApplicationUser> _userManager { get; }

        //LOG IN USER DETAILS
        public SignInManager<ApplicationUser> _signInManager { get; }
        public RoleManager<IdentityRole> _roleManager { get; }

        public AccountController(UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager,
                                    RoleManager<IdentityRole> roleManager,
                                    ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }





        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterUserViewModel();

            // Get the list of departments from the database and create a SelectList
            model.DepartmentList = new SelectList(_dbContext.Departments, "Id", "Name");
            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel userViewModel)
        {
            userViewModel.DepartmentList = new SelectList(_dbContext.Departments, "Id", "Name");

            if (ModelState.IsValid)
            {
                var userModel = new ApplicationUser
                {
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                    DepartmentId = userViewModel.DepartmentId,
                    DOB = userViewModel.DOB,
                    PhoneNumber = userViewModel.Phone,


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
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInUserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                //LOGIN ACTIVITY -> COOKIE [Roles and Claims]
                var result = await _signInManager.PasswordSignInAsync(userViewModel.UserName, userViewModel.Password, userViewModel.RememberMe, false);
                //COOKIE WILL BE CREATED and TRANSFER TO THE CLIENT
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid LogIn Credentials");

            }

            return View(userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LogIn");
        }






    }
}
