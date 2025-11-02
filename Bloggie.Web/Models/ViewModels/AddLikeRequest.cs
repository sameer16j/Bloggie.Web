namespace Bloggie.Web.Models.ViewModels
{
    public class AddLikeRequest
    {
        
        public Guid BlogPostId { get; set; }  //Id of the blog

        public Guid UserId { get; set; }  //who user is liking blog
    }
}
