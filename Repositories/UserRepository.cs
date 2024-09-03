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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AuthDemoAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserRepository(DataContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<int> Add(CNewUserDto newUserData)
        {
            CPasswordHelper.CreatePasswordHash(newUserData.Password, out byte[] passwordHash, out byte[] passwordSalt);

            CAppUser newUser = new()
            {
                UserName = newUserData.UserName,
                Email = newUserData.EMail,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            var alreadyExists = await _dbContext.Users.AnyAsync(s => s.UserName.ToLower()==newUser.UserName.ToLower()
                                        || (s.Email != null && newUser.Email!= null && s.Email.ToLower() == newUser.Email.ToLower()));

            if (!alreadyExists)
            {
                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                return newUser.Id;
            }
            else
            {
                throw new Exception("User with same name or email id already exists");
            }
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(user != null) {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else 
            {
                return false;
            }
        }

        public async Task<ICollection<CAppUser>> GetUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<string> Login(CLoginDto loginData)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginData.UserName.ToLower());
            if(user != null)
            {
                bool passwordValid = CPasswordHelper.VerifyPasswordHash(loginData.Password, user.PasswordHash, user.PasswordSalt);
                return GenerateJwtToken(user);
            }
            else 
            {
                throw new Exception("Username password dont match");
            }
        }

        private string GenerateJwtToken(CAppUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"])),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}