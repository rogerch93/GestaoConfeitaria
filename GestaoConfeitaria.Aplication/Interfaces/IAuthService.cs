using GestaoConfeitaria.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoConfeitaria.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(UserLoginDto loginDto);
        Task<RegisterResult> RegisterAsync(RegisterRequestDto registerDto);
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserDto? User { get; set; }
    }

    public class RegisterResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? UserId { get; set; }
    }
}
