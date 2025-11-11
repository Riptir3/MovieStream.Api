using MovieStream.Api.Models.Entities;

namespace MovieStream.Api.Models.DTOs
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public User? User { get; set; }

        public string? token { get; set; }
    }
}
