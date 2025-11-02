using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]   // RBAC : Authorize attribute blocks the access to  this class methods ,you have to login in first as Admin or Superadmin
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository,IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        //Get all Tags in dropdown list 
        public async Task<IActionResult> Add() 
        {
            //get tags from Repository
            var tags = await tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest 
            { 
             Tags = tags.Select(x => new SelectListItem { Text = x.Name,Value = x.Id.ToString()})
            };
            return View(model);
        }

        [HttpPost]
        //Add new BlogPOst 
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest) 
        {
            //Map view model to domain model
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle, 
                PublishedDate = addBlogPostRequest.PublishedDate,               
                Author=addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,

            };

            //Map Tags from selected tags 
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagAsGuid);

                if (existingTag != null)
                {
                   selectedTags.Add(existingTag);
                }
            }

            //Mapping tags back to domain model
            blogPost.Tags = selectedTags;

            await blogPostRepository.AddAsync(blogPost);

            return RedirectToAction("Add");
        }

        [HttpGet]
        //Get all blogposts
        public async Task<IActionResult> List() 
        { 
            //Call the repository
            var blogPosts = await blogPostRepository.GetAllAsync();
          return View(blogPosts);
        }

        [HttpGet]
        // Fetch exsisting data when click on Edit button
        public async Task<IActionResult> Edit(Guid id) 
        {
            //Fetch the result from the repository
            var blogPost = await blogPostRepository.GetAsync(id);

            var tagsDomainModel = await tagRepository.GetAllAsync();
            
            if (blogPost != null)
            {
                //map the domain model into View model
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    Author = blogPost.Author,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    //to show which tags user selected at the time of new blogpost
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    //String array
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()

                };

                return View(model);
            }

            //Pass data to view 
            return View(null);
        }

        [HttpPost]
        // Update the Blog Post
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest) 
        {
            // Map view model back to domain model
            var blogPostDomainMOdel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                Author = editBlogPostRequest.Author,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                ShortDescription = editBlogPostRequest.ShortDescription,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Visible = editBlogPostRequest.Visible
            };

            //Map tags into domain model
            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags) 
            {
                if (Guid.TryParse(selectedTag,out var tag))
                {
                    //if user has selected one tag,it will find that tag 
                    var foundTag = await tagRepository.GetAsync(tag);  

                    if (foundTag != null)
                    {
                        // and add it to the selected list
                        selectedTags.Add(foundTag);
                    }
                }
            }

            blogPostDomainMOdel.Tags = selectedTags;

            // Submit information to repository to update
            var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainMOdel);

                if (updatedBlog != null)
                {
                    //Show success notification
                    return RedirectToAction("List");
                }

                //redirect to GET
                return RedirectToAction("Edit");
            }

        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest) 
        {
            //Talk to repository to delete that blogs post and tags
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            //display the response
            if (deletedBlogPost != null) 
            {
                //Show succes notification
                return RedirectToAction("List");
            }

            
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id});
        
        }

    }
}
