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
        public async Task<IActionResult> Delete(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            var userlist = await _userManager.DeleteAsync(user);
            return RedirectToAction(controllerName: "User", actionName: "GetAllUsers"); // reload the getall page it self
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

        /*        [HttpGet]
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
                }*/


        // Get the list of departments from the database and create a SelectList



        /*        [HttpGet]
                public async Task<IActionResult> Update(string userId)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user == null)
                    {
                        return NotFound(); // or some other error handling
                    }
                    var roles = await _userManager.GetRolesAsync(user);
                    var allRoles = _roleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
                    var departments = await _dbContext.Departments.ToListAsync(); // Get the list of departments
                    var userViewModel = new EditUserViewModel()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        Email = user.Email,
                        LastName = user.LastName,
                        Roles = (IList<string>)allRoles,
                        Phone = user.PhoneNumber,
                        DOB = (DateTime)user.DOB,
                        SelectedRole = roles.First(),
                        DepartmentId = user.DepartmentId,
                        DepartmentList = new SelectList(departments, "Id", "Name"), // Create the SelectLis
                        AllRoles = _roleManager.Roles.Select(r => new SelectListItem
                        {
                            Text = r.Name,
                            Value = r.Name
                        })

                    };
                    userViewModel.Roles = roles;
                    userViewModel.AllRoles = allRoles;
                    userViewModel.DepartmentList = new SelectList(departments, "Id", "Name", user.DepartmentId);
                    return View(userViewModel);
                }*/



        /*        [HttpGet]
                public async Task<IActionResult> Update(string userId)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    var role = await _userManager.GetRolesAsync(user);
                    var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                    var roles = await _dbContext.Roles.ToListAsync();
                    var departments = await _dbContext.Departments.ToListAsync(); // Get the list of departments
                    var userViewModel = new EditUserViewModel()


                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        Email = user.Email,
                        LastName = user.LastName,
                        Roles = role,
                        Phone = user.PhoneNumber,
                        DOB = (DateTime)user.DOB,
                        RoleList = new SelectList(roles, "Id", "Name"),
                        DepartmentId = user.DepartmentId,
                        DepartmentList = new SelectList(departments, "Id", "Name") // Create the SelectLis

                    };
                    return View(userViewModel);
                }*/


        /*        [HttpPost]
                public async Task<IActionResult> Update(string userId, List<string> userRoles)
                {
                    // Get the user by Id
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Get the list of roles assigned to the user
                    var roles = await _userManager.GetRolesAsync(user);

                    // Remove the user from all the current roles
                    var result = await _userManager.RemoveFromRolesAsync(user, roles);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Failed to remove roles");
                        return View(user);
                    }

                    // Add the user to the selected roles
                    result = await _userManager.AddToRolesAsync(user, userRoles);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Failed to add roles");
                        return View(user);
                    }

                    return RedirectToAction("GetAllUsers");
                }*/




        /*        [HttpPost]
                public async Task<IActionResult> Update(EditUserViewModel userViewModel)
                {
                    if (ModelState.IsValid)
                    {
                        var user = await _userManager.FindByIdAsync(userViewModel.Id);

                        if (user == null)
                        {
                            return NotFound();
                        }

                        user.FirstName = userViewModel.FirstName;
                        user.LastName = userViewModel.LastName;
                        user.Email = userViewModel.Email;
                        user.DOB = userViewModel.DOB;
                        user.PhoneNumber = userViewModel.Phone;
                        user.DepartmentId = userViewModel.DepartmentId;
                        user.Role = userViewModel.Role;

                        // Update the user in the database
                        var result = await _userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            // Remove the user from all roles and then add them back to the selected role
                            var roles = await _userManager.GetRolesAsync(user);

                            if (roles.Any())
                            {
                                var removeResult = await _userManager.RemoveFromRolesAsync(user, roles);

                                if (!removeResult.Succeeded)
                                {
                                    ModelState.AddModelError(string.Empty, "Error removing user from roles.");
                                    return View(userViewModel);
                                }
                            }

                            var role = await _roleManager.FindByNameAsync(userViewModel.Role);

                            if (role == null)
                            {
                                ModelState.AddModelError(string.Empty, "Invalid role selected.");
                                return View(userViewModel);
                            }

                            var addResult = await _userManager.AddToRoleAsync(user, role.Name);

                            if (!addResult.Succeeded)
                            {
                                ModelState.AddModelError(string.Empty, "Error adding user to role.");
                                return View(userViewModel);
                            }

                            return RedirectToAction("GetAllUsers");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }

                    return View(userViewModel);
                }*/




        [HttpGet]
        public async Task<IActionResult> Update(string userId)
        {
            /*var user = await _userManager.FindByIdAsync(userId);*/
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            var roles = await _userManager.GetRolesAsync(user);
            EditUserViewModel userViewModel = new EditUserViewModel()
            {
                /*Id = user.Id,*/
                FirstName = user.FirstName,
                Email = user.Email,
                LastName = user.LastName,
                DepartmentId = user.DepartmentId,
                Roles = roles,

            };

            ViewData["DepartmentId"] = new SelectList(_dbContext.Departments, "Id", "Name", userViewModel.DepartmentId);

            return View(userViewModel);

        }



                [HttpPost]
                public async Task<IActionResult> Update(string userId, List<string> userRoles)
                {
                    // Get the user by Id
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Get the list of roles assigned to the user
                    var roles = await _userManager.GetRolesAsync(user);

                    // Remove the user from all the current roles
                    var result = await _userManager.RemoveFromRolesAsync(user, roles);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Failed to remove roles");
                        return View(user);
                    }

                    // Add the user to the selected roles
                    result = await _userManager.AddToRolesAsync(user, userRoles);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Failed to add roles");
                        return View(user);
                    }

                    return RedirectToAction("GetAllUsers");
                }



    }
}
