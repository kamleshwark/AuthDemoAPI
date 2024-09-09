
using Microsoft.AspNetCore.Identity;

namespace AuthDemoAPI.Entities.User
{
    public class CUserRoleMap:IdentityUserRole<int>
    {
        public CAppUser User { get; set; } = null!;
        public CRole Role { get; set; } = null!;
    }
}