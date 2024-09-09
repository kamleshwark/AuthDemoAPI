using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthDemoAPI.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthDemoAPI.Data
{
    public class Seeder
    {
        public static async Task SeedRoles(RoleManager<CRole> roleManager)
        {
            var roles = new List<CRole>()
                {
                    new() {Name="Admin"},
                    new() {Name="Updater"},
                    new() {Name="FKManager"},
                    new() {Name="ProjectManager"},
                    new() {Name="TopManagement"},
                };
            
            var newRoles = (from role in roles
                            join dbRole in roleManager.Roles on role.Name equals dbRole.Name into B
                            from b in B.DefaultIfEmpty()
                            where b is null
                            select role
            ).ToList();

            foreach (var role in newRoles)
            {
                await roleManager.CreateAsync(role);
            }

            var roleNames = roles.Select(r => r.Name).ToList();
            var deletedRoles = await
            (
                roleManager.Roles
                .Where(r => !roleNames.Contains(r.Name))
            ).ToListAsync();

            foreach (var role in deletedRoles)
            {
                await roleManager.DeleteAsync(role);
            }
        }
        public static async Task SeedUsers(UserManager<CAppUser> userManager)
        {
            var usersExist = await userManager.Users.AnyAsync();
            if(usersExist)
            {
                return;
            }

            CAppUser user = new(){UserName="admin"};
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Admin");
            return;
        }
    }
}