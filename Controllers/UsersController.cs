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
                var token = await _repo.Login(loginData);
                return Ok(new {token});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("All")]
        public async Task<ActionResult<ICollection<CAppUser>>> GetAll()
        {
            var allUsers = await _repo.GetUsers();
            return Ok(allUsers);
        }

        [Authorize]
        [HttpPost("new")]
        public async Task<ActionResult<int>> AddNewUser(CNewUserDto newUserData)
        {
            try
            {
                int newUserId = await _repo.Add(newUserData);
                return Ok(newUserId);
            }
            catch (System.Exception ex)
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
    }
}