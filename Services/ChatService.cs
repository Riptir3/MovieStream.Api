using Microsoft.Extensions.Caching.Memory;
using MovieStream.Api.Models.DTOs;
using MovieStream.Api.Models.Entities;

namespace MovieStream.Api.Services
{
    public class ChatService
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly MovieService _movieService;
        public ChatService(IConfiguration configuration, MovieService movieService, IMemoryCache cache)
        {
            _configuration = configuration;
            _movieService = movieService;
            _cache = cache;
        }

        public async IAsyncEnumerable<string> AskAiStream(ChatRequest request)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:streamGenerateContent?key={_configuration["AI:ApiKey"]}";

            var moviesContext = await _cache.GetOrCreateAsync("movies_for_ai", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                var movies = await _movieService.GetAllAsync();
                var edittedMOvies = movies.Select(m => $"{m.Title} ({m.Id}, {m.Category}, {m.Director})").ToList();
                return string.Join(", ", edittedMOvies);
            });

            string systemPrompt = $@"
                Te egy filmes asszisztens vagy. 
                KIZÁRÓLAG a következő listában szereplő filmekből ajánlhatsz: [{moviesContext}].
                Ha a felhasználó olyat kér, ami nincs a listában, udvariasan mondd meg, hogy az jelenleg nem elérhető nálunk, 
                és ajánlj helyette valamit a listából. 
                FONTOS: Ha filmet említesz, MINDIG alakítsd linkké a címét így: [Film Címe](http://localhost:5173/movies/ID) 
                Az ID helyére a film adatbázis azonosítóját írd.
                A válaszodban használj Markdown formázást (listák, félkövér szöveg).";

            using var client = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(new
                {
                    contents = new[] {
                new {
                    role = "user",
                    parts = new[] { new { text = systemPrompt + "\n\n Felhasználó kérdése: " + request.Message } }
                }
            }
                })
            };

            using var response = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                string? textToEmit = null;

                if (line.Contains("\"text\": \""))
                {
                    try
                    {
                        int start = line.IndexOf("\"text\": \"") + 9;
                        int end = line.LastIndexOf("\"");
                        if (end > start)
                        {
                            string content = line.Substring(start, end - start);
                            textToEmit = System.Text.RegularExpressions.Regex.Unescape(content);
                        }
                    }
                    catch { }
                }

                if (textToEmit != null)
                {
                    yield return textToEmit;
                }
            }
        }
    }
}
