using Bloggie.Web.Models;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bloggie.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;

        public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;  //constructor injection
            this.tagRepository = tagRepository;
        }

        //[HttpGet("generate-hash")]
        //public IActionResult GenerateHash() 
        //{
        //    var user = new IdentityUser { UserName = "superadmin@teachbook.com" };

        //    var hasher = new PasswordHasher<IdentityUser>();

        //    var hash = hasher.HashPassword(user,"Superadmin@123");

        //    return Content(hash); //returns plain text
        //}


        public async Task<IActionResult> Index()
        {
            //Get all blogspost
            var blogPosts = await blogPostRepository.GetAllAsync();

            //get all tags 
            var tags = await tagRepository.GetAllAsync();

            //Now here we are calling two different repositories at a time so you should create third view model which combines blogs and tags list

            //mapping both lists to homeviewmodel 
            var model = new HomeViewModel
            {
                BlogPosts = blogPosts,
                Tags = tags
            };

            return View(model);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
