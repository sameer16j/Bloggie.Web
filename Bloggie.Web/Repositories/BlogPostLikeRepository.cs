
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostLikeRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        //to fetch total likes from blogpostlike table on the basis of blogpostid
        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);
        }

        //to add likes of logged user 
        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await bloggieDbContext.BlogPostLike.AddAsync(blogPostLike);
            await bloggieDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        //when user refreshes the page user get chance again to hit the like button. TO avoid this we will get status whether user has earlier liked this post or not.
        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostLike.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

    }
}
