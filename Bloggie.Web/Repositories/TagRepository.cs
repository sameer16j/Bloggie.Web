using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        //to display list of tags 
        public async Task<IEnumerable<Tag>> GetAllAsync(
            string? searchQuery,
            string? sortBy,
            string? sortDirection,
            int pageNumber,
            int pageSize
            )
        {
            var query = bloggieDbContext.Tags.AsQueryable();  //convert db set (Tags) to Queryable
            //var query is now list of items that we can query

            //Filtering
            if (string.IsNullOrWhiteSpace(searchQuery) == false) //if searchQuery is not null
            {
                query = query.Where(x => x.Name.Contains(searchQuery) || x.DisplayName.Contains(searchQuery));
            }

            //Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)  //if sortBy is not null
            {
                var isDesc = string.Equals(sortDirection,"Desc",StringComparison.OrdinalIgnoreCase); //check sortdirection is ascending or descending

                //if sortby name 
                if (string.Equals(sortBy,"Name",StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x=>x.Name);
                }
                //if sortby displayname 
                if (string.Equals(sortBy,"DisplayName",StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName);
                }
            }

            //Pagination formula 

            //Skip 0 records Take 5 records -> 1st Page of 5 records
            //Skip 5 records Take next 5 records-> 2nd Page of 5 records
            // here pageSize will be choosen by User
            var skipRecords = (pageNumber - 1) * pageSize;   //Eg. 1-1 =0   >>> 0 * 5 = 0  >>> skipRecords=0
            query = query.Skip(skipRecords).Take(pageSize);  // Eg. Skip 0 records Take 5 records 


            return await query.ToListAsync();

           // return await bloggieDbContext.Tags.ToListAsync();
        }

        //Methods from interface were defined here
        public async Task<Tag> AddAsync(Tag tag)
        {
            await bloggieDbContext.Tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(id);

            if (existingTag != null)
            {
                bloggieDbContext.Tags.Remove(existingTag);
                await bloggieDbContext.SaveChangesAsync();
                return existingTag;
            }

            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsyncOld()
        {
            return await bloggieDbContext.Tags.ToListAsync();
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id); //LINQ
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);  //LINQ

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                //save changes
                await bloggieDbContext.SaveChangesAsync();

                return existingTag;
            }

            return null;

        }

        //get count of records for pagination 
        public async Task<int> CountAsync()
        {
            return await bloggieDbContext.Tags.CountAsync();
        }
    }
}
