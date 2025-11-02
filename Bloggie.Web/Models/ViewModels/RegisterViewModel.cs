using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class RegisterViewModel
    {
        //Data Anotations  like Required,EmailAddress,MinLength are used to validate these properties
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6,ErrorMessage ="Password has to be atleast 6 characters")]  //customize validation message
        public string Password { get; set; }
    }
}
