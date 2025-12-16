using Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
