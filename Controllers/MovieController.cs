using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStream.Api.Attributes;
using MovieStream.Api.Exceptions;
using MovieStream.Api.Models.DTOs;
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
        private readonly IMapper _mapper;

        public MovieController(MovieService movieService, IMapper mapper)
        {
            _movieService = movieService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Movie>>> GetAll() =>
            await _movieService.GetAllAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Movie>> GetById(string id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null) throw new NotFoundException("Movie", id);
            return movie;
        }

        [AdminOnly]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MovieDto movieDto)
        {
            var movie = _mapper.Map<Movie>(movieDto);
            await _movieService.CreateAsync(movie);

            return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
        }

        [AdminOnly]
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] MovieDto movieDto)
        {
            var existingMovie = await _movieService.GetByIdAsync(id);
            if (existingMovie == null) throw new NotFoundException("Movie", id);

            _mapper.Map(movieDto, existingMovie);

            await _movieService.UpdateAsync(existingMovie);
            return NoContent();
        }

        [AdminOnly]
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null) throw new NotFoundException("Movie", id);

            await _movieService.RemoveAsync(id);
            return NoContent();
        }
    }
}
