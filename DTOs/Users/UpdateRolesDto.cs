using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemoAPI.DTOs.Users
{
    public class CUpdateRolesDto
    {
        public int Id { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}