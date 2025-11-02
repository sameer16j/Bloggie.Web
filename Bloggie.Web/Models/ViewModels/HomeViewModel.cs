using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        //this home view model is binded in a single view to access both the lists (i.e. BlogPosts and Tags) 
        public IEnumerable<BlogPost> BlogPosts { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }
}
