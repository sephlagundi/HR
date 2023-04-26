using HRWeb.Data;
using HRWeb.Models;
using HRWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                        DOB = (DateTime)user.DOB,
                        Phone = user.PhoneNumber,
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
        public async Task<IActionResult> Delete(string userId, bool? confirmed = false)
        {
            if (!confirmed.GetValueOrDefault())
            {
                return View("DeleteConfirmation", userId);
            }

            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            var userlist = await _userManager.DeleteAsync(user);
            TempData["AlertMessage"] = "Employee deleted successfully";
            return RedirectToAction(controllerName: "User", actionName: "GetAllUsers");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string userId)
        {
            return RedirectToAction(nameof(Delete), new { userId = userId, confirmed = true });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new RegisterUserViewModel();

            // Get the list of departments from the database and create a SelectList
            model.DepartmentList = new SelectList(_dbContext.Departments, "Id", "Name");
            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterUserViewModel userViewModel)
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
                    TempData["AlertMessage"] = "Employee added successfuly";
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
                    /*await _signInManager.SignInAsync(userModel, isPersistent: false);*/
                    return RedirectToAction("GetAllUsers", "User");
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
            /*var user = await _userManager.FindByIdAsync(userId);*/
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            var roles = await _userManager.GetRolesAsync(user);
            EditUserViewModel userViewModel = new EditUserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DOB = (DateTime)user.DOB,
                Phone = user.PhoneNumber,
                DepartmentId = user.DepartmentId,
                Roles = roles,

            };

            ViewData["DepartmentId"] = new SelectList(_dbContext.Departments, "Id", "Name", userViewModel.DepartmentId);
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Id", "Name", userViewModel.Roles);


            return View(userViewModel);
        }



        [HttpPost]
        public async Task<IActionResult> Update(EditUserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var oldValue = await _userManager.FindByIdAsync(user.Id.ToString());

                {
                    oldValue.Id = user.Id; // make sure this is set correctly
                    oldValue.FirstName = user.FirstName;
                    oldValue.LastName = user.LastName;
                    oldValue.Email = user.Email;
                    oldValue.DOB = (DateTime)user.DOB;
                    oldValue.PhoneNumber = user.Phone;
                    oldValue.DepartmentId = user.DepartmentId;
                    oldValue.Roles = user.Roles;

                    // update the user object
                    var result = await _userManager.UpdateAsync(oldValue);

                    if (result.Succeeded)
                    {
                        // add roles to user
                        var roles = await _userManager.GetRolesAsync(oldValue);
                        /*                        await _userManager.RemoveFromRolesAsync(oldValue, roles);
                                                await _userManager.AddToRolesAsync(oldValue, user.Roles);*/

                        TempData["AlertMessage"] = "Employee details updated successfully";
                        return RedirectToAction("GetAllUsers", "User");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            ViewData["DepartmentId"] = new SelectList(_dbContext.Departments, "Id", "Name", user.DepartmentId);
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Id", "Name", user.Roles);

            return View(user);
        }
    }
}

