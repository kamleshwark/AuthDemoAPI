using Microsoft.AspNetCore.Identity;

namespace AuthDemoAPI.Entities.User
{
    public class CRole:IdentityRole<int>
    {
        public ICollection<CUserRoleMap> UserRoleMaps { get; set; } = [];
    }
}