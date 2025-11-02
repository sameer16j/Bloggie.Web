using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostCommentRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        //to insert comment of user in database 
        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await bloggieDbContext.BlogPostComment.AddAsync(blogPostComment);
            await bloggieDbContext.SaveChangesAsync();
            return blogPostComment;
        }

        //to fetch all comments of this blog
        public async  Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostComment.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
