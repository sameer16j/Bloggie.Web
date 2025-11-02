using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Models.ViewModels
{
    public class BlogDetailsViewModel
    {
        //this view model is for blogpost
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }

        //Navigation Property
        public ICollection<Tag> Tags { get; set; }

        public int TotalLikes { get; set; }

        //to check wether user has liked this blog or not
        public bool Liked { get; set; }
        //to add user comment in database 
        public string CommentDescription { get; set; }

        public IEnumerable<BlogComment> Comments { get; set; }
    }
}
