using Microsoft.AspNetCore.Mvc;
using MovieStream.Api.Models.DTOs;
using MovieStream.Api.Services;

namespace MovieStream.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;
        public ChatController(ChatService chatService)
        { 
            _chatService = chatService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskAi([FromBody] ChatRequest request)
        {
            var res = await _chatService.AskAi(request);

            if (!res.Success) return BadRequest(res.Message);

            return Ok(res.Message);
        }
    }
}
