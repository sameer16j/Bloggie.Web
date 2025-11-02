using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Bloggie.Web.Models.Domain;
using System.Data;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        //This API controller is to handle like of user

        private readonly IBlogPostLikeRepository blogPostLikeRepository;

        public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
        {
            this.blogPostLikeRepository = blogPostLikeRepository;
        }

        //  Add user likes to BlogPostLike table
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeReuqest)
        {
            //Mapping properties of view model(AddLikeRequest)  with domain model( BlogPostLike) 
            var model = new BlogPostLike
            {
                BlogPostId = addLikeReuqest.BlogPostId,
                UserId = addLikeReuqest.UserId
            };
 
            await blogPostLikeRepository.AddLikeForBlog(model);

            return Ok();
        }


        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]   // asp.net will auto map varibale blogPostId in route and pass it as parameter in GetTotalLikesForBlog
        public async Task<IActionResult> GetTotalLikesForBlog([FromRoute] Guid blogPostId) 
        {
            var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPostId);
            return Ok(totalLikes);
        }
    }
}
