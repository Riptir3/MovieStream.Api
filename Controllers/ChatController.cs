using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using MovieStream.Api.Models.DTOs;
using MovieStream.Api.Services;

namespace MovieStream.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;
        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("ask-stream")]
        [Authorize] 
        public async Task GetAiStream([FromBody] ChatRequest request)
        {
            Response.ContentType = "text/event-stream";

            await foreach (var chunk in _chatService.AskAiStream(request))
            {
                await Response.WriteAsync($"data: {chunk.Replace("\n", "\\n")}\n\n");
                await Response.Body.FlushAsync();

                await Task.Delay(30);
            }
        }
    }
}

