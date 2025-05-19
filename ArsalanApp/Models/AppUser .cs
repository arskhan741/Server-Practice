using Microsoft.AspNetCore.Identity;

namespace ArsalanApp.Models
{
    public class AppUser : IdentityUser
    {
        public string ContactNo { get; set; } = String.Empty;
        public DateOnly? DateOfBith { get; set; }
    }
}
