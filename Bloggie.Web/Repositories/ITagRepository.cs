using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    //Interface: In which we have declared methods.Those methods will now be implemented/defined in TagRepository.cs
    public interface ITagRepository  
    {
        // Task<IEnumerable<Tag>> GetAllAsync();

        //Get list of all records
        Task<IEnumerable<Tag>> GetAllAsync(
            string? searchQuery = null, 
            string? sortBy = null,
            string? sortDirection= null,
            int pageNumber =1,
            int pageSize=100
            );   // pageSize and pageNumber are default 

        Task<Tag?> GetAsync(Guid id);

        Task<Tag> AddAsync(Tag tag);

        Task<Tag?> UpdateAsync(Tag tag);

        Task<Tag?> DeleteAsync(Guid id);

        //Get count of records for pagination
        Task<int> CountAsync();
    }
}
