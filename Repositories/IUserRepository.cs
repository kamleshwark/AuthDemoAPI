using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthDemoAPI.DTOs.Users;
using AuthDemoAPI.Entities.User;

namespace AuthDemoAPI.Repositories
{
    public interface IUserRepository
    {
        public Task<ICollection<CAppUser>> GetUsers();
        public Task<int> AddUser(CNewUserDto newUserData);
    }
}