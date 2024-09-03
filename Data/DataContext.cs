
using AuthDemoAPI.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace AuthDemoAPI.Data
{
    public class DataContext: DbContext
    {
       public DataContext(DbContextOptions<DataContext> options) : base(options) { }

       public DbSet<CAppUser> Users { get; set; }
       public DbSet<CRole> Roles { get; set; }
       public DbSet<CUserRoleMap> UserRoleMaps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CUserRoleMap>()
                .HasKey(e => new{e.RoleId, e.UserId});
        }
    }
}