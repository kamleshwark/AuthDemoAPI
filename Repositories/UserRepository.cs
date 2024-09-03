using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthDemoAPI.Data;
using AuthDemoAPI.DTOs.Users;
using AuthDemoAPI.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace AuthDemoAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dbContext;
        public UserRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
            
        }

        public async Task<int> AddUser(CNewUserDto newUserData)
        {
            CAppUser newUser = new()
            {
                UserName = newUserData.UserName,
                Email = newUserData.EMail,
                PasswordHash = new byte[1],
                PasswordSalt = new byte[1]
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

        public async Task<ICollection<CAppUser>> GetUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }
    }
}