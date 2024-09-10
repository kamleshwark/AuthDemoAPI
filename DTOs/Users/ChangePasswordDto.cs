using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemoAPI.DTOs.Users
{
    public class CChangePasswordDto
    {
        public int Id { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}