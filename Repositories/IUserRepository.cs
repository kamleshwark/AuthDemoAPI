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
        public Task<List<CUserToReturnDto>> GetUsers();
        public Task<int> Register(CNewUserDto newUserData);

        public Task<bool> UpdateRoles(CUpdateRolesDto roleData);

        public Task<bool> Delete(int id);

        public Task<string> Login(CLoginDto loginData);

        public Task<bool> ChangeActiveState(int id, bool newState);

        public Task<bool> ChangeDeletedState(int id, bool newState);

        public Task<bool> ResetPassword(int id);
        public Task<bool> ChangePassword(CChangePasswordDto data);
    }
}