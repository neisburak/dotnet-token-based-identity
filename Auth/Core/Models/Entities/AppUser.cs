using Microsoft.AspNetCore.Identity;

namespace Core.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public string? City { get; set; }
    }
}