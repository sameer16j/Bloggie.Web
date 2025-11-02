namespace Bloggie.Web.Models.ViewModels
{
    //Now these model properties will be binded in View(Edit.cshtml)  
    public class EditTagRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
