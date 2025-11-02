namespace Bloggie.Web.Models.ViewModels
{
    public class UserViewModel
    {
        //to show list of users on screen
        public List<User> Users { get; set; }

        //using this properties when user submits the create  new user form
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool AdminRoleCheckbox { get; set; }
    }
}
