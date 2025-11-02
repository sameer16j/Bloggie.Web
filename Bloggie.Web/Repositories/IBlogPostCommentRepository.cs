using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostCommentRepository
    {
        //add comment of a user in Database 
        Task<BlogPostComment>AddAsync(BlogPostComment blogPostComment);
        
        //Fetch all comments of this blog by id
        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId);

    }
}
