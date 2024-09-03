using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet("Hello")]
        public ActionResult<string> Hello()
        {
            return Ok("Hello From Students Controller");
        }
    }
}