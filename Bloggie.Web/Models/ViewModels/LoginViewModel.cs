using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        //view model binding for two way binding.Passing data from view to controller
        //Data Anotations  like Required,MinLength are used to validate these properties
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(6,ErrorMessage = "Password has to be atleast 6 characters")]  //customize validation message
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
