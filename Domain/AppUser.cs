using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
    }
}