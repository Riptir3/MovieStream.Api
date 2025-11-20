using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieStream.Api.Models.DTOs;
using MovieStream.Api.Models.Entities;
using MovieStream.Api.Services;
using System.Security.Claims;

namespace MovieStream.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovieRequestController : ControllerBase
    {
        private readonly MovieRequestService _movieRequestService;

        public MovieRequestController(MovieRequestService movieRequestService)
        {
            _movieRequestService = movieRequestService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendRequest(MovieRequestDto movieRequestDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var movieRequest = new MovieRequest
            {
                Title = movieRequestDto.Title,
                Director = movieRequestDto.Director,
                ReleaseYear = movieRequestDto.ReleaseYear,
                Comment = movieRequestDto.Comment,
                UserId = userId
            };

            await _movieRequestService.CreateAsync(movieRequest);

            return Ok(new { message = "Movie request sent successfully!" });
        }
    }
}
