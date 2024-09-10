using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthDemoAPI.Data;
using AuthDemoAPI.DTOs.Users;
using AuthDemoAPI.Entities.User;
using AuthDemoAPI.Utility;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AuthDemoAPI.Repositories
{
    public class UserRepository(UserManager<CAppUser> _userManager, IConfiguration _configuration) : IUserRepository
    {
        private async Task<bool> UserExists(string userName)
        {
            return await _userManager.Users.AnyAsync(u => u.NormalizedUserName == userName.ToUpper());
        }
        public async Task<int> Register(CNewUserDto newUserData)
        {
            if(await UserExists(newUserData.UserName))
            {
                throw new Exception("Username already taken");
            }
            CAppUser newUser = new()
            {
                UserName = newUserData.UserName.ToLower(),
                Email = newUserData.EMail,
            };

            var result = await _userManager.CreateAsync(newUser, newUserData.Password);
            if(result.Succeeded) {
                await _userManager.AddToRolesAsync(newUser, newUserData.Roles);
                return newUser.Id;
            }
            else
            {
                var errorList = result.Errors.ToList();
                string error = "";
                foreach (var er in errorList)
                {
                    error += er.Code +"-->"+er.Description + "\n";
                }
                throw new Exception(error);
            }
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return true;
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public async Task<List<CUserToReturnDto>> GetUsers()
        {
            var users = await _userManager.Users.Select(u => new CUserToReturnDto
            {
                Id = u.Id,
                nm = u.UserName,
                fnm = u.FullName,
                em = u.Email,
                r = u.UserRoleMaps.Select(m => m.Role.Name ?? "").ToList()
            }).ToListAsync();
            return users;
        }

        public async Task<string> Login(CLoginDto loginData)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == loginData.UserName.ToUpper()&&u.IsActive&&!u.MarkedDeleted);
            if (user != null)
            {
                if (await _userManager.IsLockedOutAsync(user))
                {
                    throw new Exception($"User is locked out. Try again after {user.LockoutEnd}");
                }
                var passwordValid = await _userManager.CheckPasswordAsync(user, loginData.Password);
                if (passwordValid)
                {
                    user.LastLoginAt = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                    await _userManager.ResetAccessFailedCountAsync(user);
                    return await GenerateJwtToken(user);
                }
                else {
                    await _userManager.AccessFailedAsync(user);
                    throw new Exception("Incorrect password");
                }
            }
            else
            {
                throw new Exception("Username does not exist");
            }
        }

        private async Task<string> GenerateJwtToken(CAppUser user)
        {
            if(user.UserName == null) 
            {
                throw new Exception("sername required");
            }
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? ""));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var tokenOptions = new JwtSecurityToken(
                            issuer: jwtSettings["Issuer"],
                            audience: jwtSettings["Audience"],
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"] ?? "1")),
                            signingCredentials: signingCredentials
                        );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<bool> UpdateRoles(CUpdateRolesDto roleData)
        {
            var user = await _userManager.FindByIdAsync(roleData.Id.ToString());
            if(user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.AddToRolesAsync(user, roleData.Roles.Except(roles));
                await _userManager.RemoveFromRolesAsync(user, roles.Except(roleData.Roles));
            }
            else
            {
                throw new Exception("User not found");
            }

            return true;
        }

        public async Task<bool> ChangeActiveState(int id, bool newState)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if(user != null)
            {
                user.IsActive = newState;
                await _userManager.UpdateAsync(user);
                return true;
            }
            else 
            {
                throw new Exception("User doesnt exist");
            }
        }

        public async Task<bool> ChangeDeletedState(int id, bool newState)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if(user != null)
            {
                user.MarkedDeleted = newState;
                await _userManager.UpdateAsync(user);
                return true;
            }
            else 
            {
                throw new Exception("User doesnt exist");
            }
        }

    }
}