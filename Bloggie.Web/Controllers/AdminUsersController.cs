using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Bloggie.Web.Controllers
{

    [Authorize(Roles = "Admin")] // RBAC : Authorize attribute blocks the access to  this class methods ,you have to login in first as Admin or Superadmin
    public class AdminUsersController : Controller
    {
        //dependency injection
        private readonly IUserRepository userRepository;  //to fetch list of users 
        private readonly UserManager<IdentityUser> userManager; // for create new user

        public AdminUsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }

        //Fetch the list of users except superadmin role
        [HttpGet]
        public async Task<IActionResult> List()
        {
            // Get all users from the repository (excluding superadmin)
            var users = await userRepository.GetAll();

            // Loop through each IdentityUser and convert it to a simpler User model
            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();

            foreach (var user in users)
            {
                usersViewModel.Users.Add(new Models.ViewModels.User
                {
                    Id = Guid.Parse(user.Id),
                    Username = user.UserName,
                    EmailAddress = user.Email
                });
            }
            return View(usersViewModel);
        }

        //to create new user
        [HttpPost]
        public async Task<IActionResult> List(UserViewModel request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email
            };

            var identityResult = await userManager.CreateAsync(identityUser, request.Password);

            if (identityResult is not null)
            {
                //checks new user created or not
                if (identityResult.Succeeded)
                {
                    var roles = new List<string> { "User" }; //Add "User" role in roles list

                    if (request.AdminRoleCheckbox)
                    {
                        roles.Add("Admin"); //Add "Admin" role in roles list
                    }

                    //assign roles to this newly created user
                    identityResult = await userManager.AddToRolesAsync(identityUser, roles);

                    //checks roles are assigned to new user
                    if (identityResult is not null && identityResult.Succeeded)
                    {
                        return RedirectToAction("List", "AdminUsers");   //(action name, controller name)
                    }
                }
            }
            return View();
        }

        //Delete users
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user is not null)
            {
                var identityResult = await userManager.DeleteAsync(user);

                if (identityResult is not null && identityResult.Succeeded)
                {
                    return RedirectToAction("List", "AdminUsers");
                }
            }           
            return View();
        }


    }
}
