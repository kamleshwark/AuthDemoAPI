namespace AuthDemoAPI.Entities.User
{
    public class CAppUser
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public ICollection<CRole> Roles { get; set; }

        public CAppUser()
        {
            Roles = new List<CRole>();
        }
    }
}