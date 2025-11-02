using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostRepository
    {
        //get all blogpost
        Task<IEnumerable<BlogPost>> GetAllAsync();

        //get single blogpost
        Task<BlogPost?> GetAsync(Guid id);

        //get blog whole info to display to user
        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);

        Task<BlogPost> AddAsync(BlogPost blogPost);

        Task<BlogPost?> UpdateAsync(BlogPost blogPost);

        Task<BlogPost?> DeleteAsync(Guid id);
    }
}
