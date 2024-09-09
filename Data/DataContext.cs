
using AuthDemoAPI.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthDemoAPI.Data
{
    public class DataContext(DbContextOptions<DataContext> options)
        : IdentityDbContext<CAppUser, CRole, int, IdentityUserClaim<int>, CUserRoleMap, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CAppUser>()
                .HasMany(u => u.UserRoleMaps)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .IsRequired();
            
            builder.Entity<CRole>()
                .HasMany(u => u.UserRoleMaps)
                .WithOne(m => m.Role)
                .HasForeignKey(m => m.RoleId)
                .IsRequired();
            
            builder.Entity<CUserRoleMap>()
                .HasKey(m => new {m.UserId, m.RoleId});
        }
    }
}