using AutoMapper;
using GestaoConfeitaria.Application.DTOs;
using GestaoConfeitaria.Application.Interfaces;
using GestaoConfeitaria.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestaoConfeitaria.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
                return Unauthorized(result.Message);

            // Login com Cookie (mantendo o comportamento original)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.User.Username),
                new Claim(ClaimTypes.Role, result.User.Role),
                new Claim("UserId", result.User.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(new { message = "Login realizado com sucesso" });
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Success)
                return BadRequest(result.Message);

            return Created($"/api/auth/users/{result.UserId}", new
            {
                result.UserId,
                registerDto.Username,
                Role = "User"
            });
        }
    }
}
