using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStream.Api.Models.Entities;
using MovieStream.Api.Services;

namespace MovieStream.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieService _movieService;

        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Movie>>> GetAll() =>
            await _movieService.GetAllAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Movie>> GetById(string id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null) return NotFound();
            return movie;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Movie movie)
        {
            await _movieService.CreateAsync(movie);
            return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Movie updatedMovie)
        {
            var existingMovie = await _movieService.GetByIdAsync(id);
            if (existingMovie == null) return NotFound();

            updatedMovie.Id = id; 
            await _movieService.UpdateAsync(id, updatedMovie);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null) return NotFound();

            await _movieService.RemoveAsync(id);
            return NoContent();
        }
    }
}
