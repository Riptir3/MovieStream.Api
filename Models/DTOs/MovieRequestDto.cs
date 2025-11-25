namespace MovieStream.Api.Models.DTOs
{
    public class MovieRequestDto
    {
        public string Title { get; set; } = null!;
        public string Director { get; set; } = null!;
        public int ReleaseYear { get; set; }
        public string? Comment { get; set; }
        public string Status { get; set; } = "Active";
    }
}
