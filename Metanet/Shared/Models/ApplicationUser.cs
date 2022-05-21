
using Microsoft.AspNetCore.Identity;

namespace Metanet.Shared.Models
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        
        public string Name { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }


    }
    public class UserRole :IdentityUserRole<string>
    {
        public   ApplicationUser User { get; set; }
        public  Role Role { get; set; }
    }

    public class Role : IdentityRole<string>
    {
        public  ICollection<UserRole> UserRoles { get; set; }
    }


}
