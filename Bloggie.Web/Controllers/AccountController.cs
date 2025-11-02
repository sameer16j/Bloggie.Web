using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Bloggie.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;  //for registration of new user
        private readonly SignInManager<IdentityUser> signInManager; //for  login & logout

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        //User Registration Page
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        //register new user
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            //Checks if RegisterViewModel is in valid state or not for server side validation.
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email
                };

                var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

                //checks new user register or not
                if (identityResult.Succeeded)
                {
                    //assign this user with  "User" role
                    var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

                    //checks user role is assigned to new user
                    if (roleIdentityResult.Succeeded)
                    {
                        return RedirectToAction("Register");
                    }

                }
            }
            
            //Show error notification
            return View();
        }

        [HttpGet]
        //user login page
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = ReturnUrl
            };
            return View(model);
        }

        [HttpPost]
        //Sign In 
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            //Checks if RegisterViewModel is in valid state or not for server side validation.
            if (!ModelState.IsValid)
            {
                return View();
            }

            var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

            if (signInResult != null && signInResult.Succeeded)
            {
                // check if returnurl is null or not. if not null then redirect to that page.
                if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");  // Index view of home controller
            }

            //Show errors
            return View();
        }

        [HttpGet]
        //Sign Out
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
