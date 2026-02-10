using Microsoft.AspNetCore.Mvc;
using MovieStream.Api.Models.DTOs;
using TaskManager.Api.Models;

namespace MovieStream.Api.Services
{
    public class ChatService
    {
        private readonly IConfiguration _configuration;
        public ChatService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Response<string>> AskAi([FromBody] ChatRequest request)
        {
            var url = $"https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash:generateContent?key={_configuration["AI:ApiKey"]}";

            using var client = new HttpClient();
            var requestBody = new
            {
                contents = new[] {
                    new {
                        parts = new[] {
                            new {
                                text = "Te egy filmes asszisztens vagy. Segíts a felhasználónak. " +
                                "MINDEN említett filmet linkelj így: [Cím](http://localhost:3000/movie/TMDB_ID). " +
                                "Fontos: Csak a TMDB ID-t használd az URL végén! " +
                                $"A felhasználó kérdése: {request.Message}"
                            }
                        }
                    }
                }
            };

            var response = await client.PostAsJsonAsync(url, requestBody);
            var result = await response.Content.ReadAsStringAsync();

            if(result == null)
            {
                return Response<string>.Fail("Bad request"); 
            }

            return Response<string>.OK(result);
        }
    }
}
