using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStream.Api.Services;
using System.Security.Claims;

namespace MovieStream.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly FavoriteService _favoriteService;

        public FavoriteController(FavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserFavorites()
        {
            var userId = GetUserId();
            var favorites = await _favoriteService.GetUserFavorites(userId);
            return Ok(favorites.Select(f => f.MovieId).ToList());
        }

        [HttpPost("add/{movieId}")]
        public async Task<IActionResult> AddFavorite(string movieId)
        {
            var userId = GetUserId();
            var added = await _favoriteService.AddFavorite(userId, movieId);
            if (!added) return BadRequest("Already exists");
            return Ok();
        }

        [HttpDelete("remove/{movieId}")]
        public async Task<IActionResult> RemoveFavorite(string movieId)
        {
            var userId = GetUserId();
            var removed = await _favoriteService.RemoveFavorite(userId, movieId);
            if (!removed) return BadRequest("Not found");
            return Ok();
        }
    }
}
