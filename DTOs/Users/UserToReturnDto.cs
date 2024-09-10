using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemoAPI.DTOs.Users
{
    public class CUserToReturnDto
    {
        public int Id { get; set; }
        //User Name
        public string? nm { get; set; }
        //Fullname
        public string? fnm { get; set; }
        //Email
        public string? em { get; set; }
        //Roles
        public List<String> r { get; set; } = [];
    }
}