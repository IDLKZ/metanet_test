using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Metanet.Shared.DTO
{

    public class UserDTO
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string UserName { get; set; }
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string NormalizedUserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string NormalizedEmail { get; set; }
         
        [Required]
        public bool EmailConfirmed { get; set; }

        [Required]
        public string PhoneNumber { get; set; }


        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public string ImageUrl { get; set; }
        public string Description { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } 

        public List<UserRolesDTO> UserRoles { get; set; }

    }

    public class UserRolesDTO
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public RoleDTO Role { get; set; }

    }


    public class RoleDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
