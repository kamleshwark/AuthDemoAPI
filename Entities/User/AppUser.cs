using Microsoft.AspNetCore.Identity;

namespace AuthDemoAPI.Entities.User
{
    public class CAppUser:IdentityUser<int>
    {
        public string? FullName { get; set; }
        public bool IsActive { get; set; } = true;
        public bool MarkedDeleted { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public ICollection<CUserRoleMap> UserRoleMaps { get; set; } = [];
        
    }
}