using Microsoft.AspNetCore.Identity;

namespace AuthenticationAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
    }
}
