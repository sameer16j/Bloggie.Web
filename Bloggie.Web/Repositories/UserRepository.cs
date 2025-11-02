using Bloggie.Web.Data;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext authDbContext;

        //Dependency Injection
        public UserRepository(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }

        //to get the list of all users except SuperAdmin
        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            //Get all users from database
             var users = await authDbContext.Users.ToListAsync();

           //Find the superadmin user
             var superAdminUser = await  authDbContext.Users.FirstOrDefaultAsync(x => x.Email == "superadmin@bloggie.com");

            //If superadmin exists, remove from the list
            if (superAdminUser != null) {
                users.Remove(superAdminUser); 
            }
            return users;
        }
            
    }
}
