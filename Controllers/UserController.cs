using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MovieStream.Api.Models.DTOs;
using MovieStream.Api.Services;

namespace MovieStream.Api.Controllers
{
    [EnableRateLimiting("AuthPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AuthService _authService;

        public UserController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            var result = await _authService.RegisterAsync(userRegisterDto);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var result = await _authService.LoginAsync(userLoginDto);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { Token = result.token, Username = result.username});
        }
    }
}
