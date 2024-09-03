using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemoAPI.DTOs.Users
{
    public class CLoginDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}