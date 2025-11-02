using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{

    public class AddTagRequest
    {
        //Data Anotations  like Required,EmailAddress,MinLength are used to validate these properties
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}
