using Microsoft.AspNetCore.Authorization;
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
    public class MovieReportController : ControllerBase
    {
        private readonly MovieReportService _movieReportService;
        private readonly MovieService _movieService;

        public MovieReportController(MovieReportService movieReportService, MovieService movieService)
        {
            _movieReportService = movieReportService;
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieReport>>> GetAll() =>
            await _movieReportService.GetAllAsync();

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] MovieReportDto movieReportDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var movie = await _movieService.GetByIdAsync(movieReportDto.MovieId);

            if (movie == null) return BadRequest(new { message = "Movie is not valid!" });

            var movieReport = new MovieReport
            {
                MovieId = movieReportDto.MovieId,
                Comment = movieReportDto.Comment,
                UserId = userId
            };

            await _movieReportService.CreateAsync(movieReport);

            return Ok(new { message = "Movie report sent successfully!" });
        }
    }
}
