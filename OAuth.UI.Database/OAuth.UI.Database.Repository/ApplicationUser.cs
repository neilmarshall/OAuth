using Microsoft.AspNetCore.Identity;

namespace OAuth.UI.Database.Repository
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
        }
    }
}
