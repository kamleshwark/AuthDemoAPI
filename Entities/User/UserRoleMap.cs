
namespace AuthDemoAPI.Entities.User
{
    public class CUserRoleMap
    {
        public int UserId { get; set; }
        public CAppUser? User { get; set; }
        public int RoleId { get; set; }
        public CRole? Role { get; set; }
    }
}