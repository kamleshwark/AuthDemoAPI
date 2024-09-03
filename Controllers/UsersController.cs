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

        [Authorize]
        [HttpGet("All")]
        public async Task<ActionResult<ICollection<CAppUser>>> GetAll()
        {
            var allUsers = await _repo.GetUsers();
            return Ok(allUsers);
        }

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

        [HttpPut("login")]
        public async Task<ActionResult<string>> Login(CLoginDto loginData)
        {
            try
            {
                var token = await _repo.Login(loginData);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}