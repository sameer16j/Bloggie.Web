using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]  
    //Routing https://localhost:7001/api/Images
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)  //constructor injection
        {
            this.imageRepository = imageRepository;
        }


        //[HttpGet]
        //public IActionResult Index() 
        //{
        //    return Ok("This is the GET Images API call");
        //}

        //This controller method will call a repository and that repository will call third party api service i.e. Cloudinary. And this service will upload images to cloud and in return we will get url.

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file) 
        {
            //call repository
           var imageURL= await imageRepository.UploadAsync(file);

            if (imageURL == null)   // check url is  not null
            {
                return Problem("Something went wrong!",null,(int)HttpStatusCode.InternalServerError);
            }

            return new JsonResult( new { link = imageURL});
        }
    }
}
