using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStream.Api.Attributes;
using MovieStream.Api.Exceptions;
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
        private readonly IMapper _mapper;

        public MovieRequestController(MovieRequestService movieRequestService, IMapper mapper)
        {
            _movieRequestService = movieRequestService;
            _mapper = mapper;
        }

        [AdminOnly]
        [HttpGet]
        public async Task<ActionResult<List<MovieRequest>>> GetAll() =>
            await _movieRequestService.GetAllAsync();

        [AdminOnly]
        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<List<MovieRequest>>> EditRequest(string id, string status)
        {
            var requestedMovie = await _movieRequestService.FindById(id);
            if (requestedMovie == null)
            {
                throw new NotFoundException("Movie Requet", id);
            }
   
            requestedMovie.Status = status;
            await _movieRequestService.UpdateAsync(requestedMovie);
            return NoContent();
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendRequest([FromBody] MovieRequestDto movieRequestDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var movieRequest = _mapper.Map<MovieRequest>(movieRequestDto);
            movieRequest.UserId = userId;

            await _movieRequestService.CreateAsync(movieRequest);

            return Ok(new { message = "Movie request sent successfully!" });
        }
    }
}
