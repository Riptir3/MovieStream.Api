using MovieStream.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MovieStream.Api.Models.DTOs
{
    public class MovieRequestDto
    {
        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = "Director is required!")]
        [MinLength(5, ErrorMessage = "Director must be at least 5 characters long!")]
        public string Director { get; set; } = null!;
        [ReleaseYear]
        public int ReleaseYear { get; set; }
        public string? Comment { get; set; }
        [AllowedValues("Active",ErrorMessage = "Value of status field is not valid!")]
        public string Status { get; set; } = "Active";
    }
}
