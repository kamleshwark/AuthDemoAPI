using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthDemoAPI.DTOs.Users;
using AuthDemoAPI.Entities.User;
using AuthDemoAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemoAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UsersController(IUserRepository repo)
        {
            _repo = repo;
        }
        [HttpGet("Hello")]
        public ActionResult<string> Hello()
        {
            return Ok("Hello From Students Controller");
        }

        [AllowAnonymous]
        [HttpPut("login")]
        public async Task<ActionResult<string>> Login(CLoginDto loginData)
        {
            try
            {
                var result = await _repo.Login(loginData);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("All")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var allUsers = await _repo.GetUsers();
                return Ok(allUsers);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<ActionResult<int>> Register(CNewUserDto newUserData)
        {
            try
            {
                int newUserId = await _repo.Register(newUserData);
                return Ok(newUserId);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateRoles")]
        public async Task<ActionResult> UpdateRoles(CUpdateRolesDto roleData)
        {
            try
            {
                var result = await _repo.UpdateRoles(roleData);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                var result = await _repo.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("activate/{id}")]
        public async Task<ActionResult<bool>> Activate(int id)
        {
            try
            {
                var result = await _repo.ChangeActiveState(id, true);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("deactivate/{id}")]
        public async Task<ActionResult<bool>> Deactivate(int id)
        {
            try
            {
                var result = await _repo.ChangeActiveState(id, false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("markDeleted/{id}")]
        public async Task<ActionResult<bool>> MarkDeleted(int id)
        {
            try
            {
                var result = await _repo.ChangeDeletedState(id, true);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("markNotDeleted/{id}")]
        public async Task<ActionResult<bool>> MarkNotDeleted(int id)
        {
            try
            {
                var result = await _repo.ChangeDeletedState(id, false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("ResetPassword/{Id}")]
        public async Task<ActionResult> ResetPassword(int id)
        {
            try
            {
                return Ok(await _repo.ResetPassword(id));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePassword(CChangePasswordDto data)
        {
            try
            {
                return Ok(await _repo.ChangePassword(data));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }
    }
}