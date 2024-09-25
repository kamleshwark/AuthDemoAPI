using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemoAPI.DTOs.Users
{
    public class CLoginResultDto
    {
        public int Code { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public List<string> Roles { get; set; } = [];
        public string? Token { get; set; }
        public string? Message { get; set; }
        public DateTime TokenExpiresOn { get; set; }
    }
}