using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]   // RBAC : Authorize attribute blocks the access to  this class methods ,you have to login in first as Admin or Superadmin
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }


        //private readonly BloggieDbContext bloggieDbContext;
        ////public BloggieDbContext _bloggieDbContext;

        //public AdminTagsController(BloggieDbContext bloggieDbContext)   //Dependency Injection
        //{
        //    this.bloggieDbContext = bloggieDbContext;
        //    //_bloggieDbContext = bloggieDbContext;   

        //}

        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

       
        [HttpPost]
        //Save New Tags 
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)   //model binding 
        {
            //call custom validation method
            ValidateAddTagRequest(addTagRequest); 

            //Checks if AddTagRequest is in valid state or not for server side validation.
            if (ModelState.IsValid == false)
            {
                return View();
            }

            var tag = new Tag
            {
                ////Model binding example: addTagRequest properties assigned to Tag model property
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            
            await tagRepository.AddAsync(tag); // now its a job of the repository to talk to the Database not the controller

            return RedirectToAction("List");

            ////Manual Binding 
            //var name = Request.Form["name"];
            //var displayName = Request.Form["displayName"];

            ////Model binding example 
            //var name = addTagRequest.Name;
            //var displayName = addTagRequest.DisplayName;

           // return View("Add");
        }

        
        [HttpGet]
        //Read Tags with filtering 
        //parameter name(searchQuery) inside List method should match with name atrribute(searchQuery) in input element of view
        public async Task<IActionResult> List(string? searchQuery,string? sortBy,string? sortDirection, int pageSize=3,int pageNumber =1) 
        {
            //use dbContext to read tags
            //var tags = await bloggieDbContext.Tags.ToListAsync();

            var totalRecords = await tagRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords /pageSize);  // total Pages = total records / pageSize

            //When user click on nxt page button and pagenumber goes above total pages then stay on current page
            if (pageNumber > totalPages)
            {
                pageNumber--;
            }

            //When user clicks on prev page button and pagenumber goes below 1 then stay on current page
            if (pageNumber<1)
            {
                pageNumber++;
            }

            //Viewbag to temporarily save the value of that search query,sortBy,sortDirection
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;

            var tags = await tagRepository.GetAllAsync(searchQuery, sortBy, sortDirection,pageNumber, pageSize);// now its a job of the repository to talk to the Database not the controller
            return View(tags);

        }


        [HttpGet]
        //Read Tags  without filtering
        public async Task<IActionResult> ListOld()
        {
            //use dbContext to read tags
            //var tags = await bloggieDbContext.Tags.ToListAsync();

            var tags = await tagRepository.GetAllAsync();// now its a job of the repository to talk to the Database not the controller
            return View(tags);

        }


        [HttpGet]
        //Edit Tags- Get exsisting data for edit 
        public async Task<IActionResult> Edit(Guid id) 
        {
            //1st method to get data through ID
            //bloggieDbContext.Tags.Find(id);

            //2nd method 
            var tag = await tagRepository.GetAsync(id); // now its a job of the repository to talk to the Database not the controller

            if (tag != null)
            {
                //model binding
                var editTagRequest = new EditTagRequest {
                Id=tag.Id,
                Name=tag.Name,
                DisplayName=tag.DisplayName
                };
                return View(editTagRequest);  //return data to Edit view
            }

            return View();
        }

   
        [HttpPost]
        //Edit Tags- save updated data in database
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest) 
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var updatedTag = await tagRepository.UpdateAsync(tag);// now its a job of the repository to talk to the Database not the controller

            if (updatedTag != null)
            {
                //Show success notification
                return RedirectToAction("List");
            }
            else 
            {
              //Show error notification
            }

            #region Old code
            //var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);  //LINQ

            //if (existingTag != null)
            //{
            //    existingTag.Name = tag.Name;
            //    existingTag.DisplayName = tag.DisplayName;

            //    //save changes
            //    await bloggieDbContext.SaveChangesAsync();

            //    //Show success notification
            //    return RedirectToAction("List");
            //    //return RedirectToAction("Edit", new { id = editTagRequest.Id });
            //}
            #endregion

            
            return RedirectToAction("Edit", new { id = editTagRequest.Id});
        }
    
        [HttpPost]
        //Delete Tags
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest) 
        {
            var deletedTag = await tagRepository.DeleteAsync(editTagRequest.Id);

            if (deletedTag != null) 
            {
                //Show sucess notification
                return RedirectToAction("List");
            }
            #region Old code
            //var tag = bloggieDbContext.Tags.Find(editTagRequest.Id);

            //if (tag != null)
            //{
            //    bloggieDbContext.Tags.Remove(tag);
            //    await bloggieDbContext.SaveChangesAsync();

            //    //Show a sucess notification
            //    return RedirectToAction("List");
            //}
            #endregion

            //Show an error notification
            return RedirectToAction("Edit", new {id = editTagRequest.Id });
        }

        //Custom Validation Method
        private void ValidateAddTagRequest(AddTagRequest request) 
        {
            if (request.Name is not null && request.DisplayName is not null)
            {
                if (request.Name == request.DisplayName)
                {
                    //AddModelError(PropertyName, error message);
                    ModelState.AddModelError("DisplayName", "Name cannot be the same as DisplayName");  
                    
                }
            }
        }
    }
}
