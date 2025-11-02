using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostLikeRepository
    {
        //Fetch total likes to that particluar blogpost
        Task<int> GetTotalLikes(Guid blogPostId);

        //when user refreshes the page user get chance again to hit the like button. TO avoid this we will get status whether user has earlier liked this post or not.
        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);

        //to add likes of logged user 
        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);
    }
}
