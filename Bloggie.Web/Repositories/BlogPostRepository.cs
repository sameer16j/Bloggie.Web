using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Bloggie.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        //Constructor injection
        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await bloggieDbContext.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog = await bloggieDbContext.BlogPosts.FindAsync(id);

            if (existingBlog != null) 
            {
                bloggieDbContext.BlogPosts.Remove(existingBlog);
                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            //Include() is used to fetch assigned  Tags to each BlogPOst
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        //Fetch data for Edit form on the basis of id
        public  async Task<BlogPost?> GetAsync(Guid id)
        {
          return await bloggieDbContext.BlogPosts.Include(x=> x.Tags).FirstOrDefaultAsync(x => x.Id == id);
             
        }

        //Fetch Single Blog info to display to user on the basis of urlHandle
        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            //Include() is used to fetch assigned  Tags to each BlogPOst
            return await bloggieDbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x=> x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await bloggieDbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x=>x.Id == blogPost.Id);

            //Mapping
            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.Author = blogPost.Author;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Tags = blogPost.Tags;
                
                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }
     
    }
}
