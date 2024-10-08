using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemoAPI.DTOs.Users
{
    public class CNewUserDto
    {
        public required string UserName { get; set; }
        public string? EMail { get; set; }
        public required string Password { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}