using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemoAPI.DTOs.Users
{
    public class CLoginResultDto
    {
        public int Code { get; set; }
        public required string Message { get; set; }
    }
}