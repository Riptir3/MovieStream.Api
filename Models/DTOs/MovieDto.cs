using MovieStream.Api.Attributes;
using MovieStream.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MovieStream.Api.Models.DTOs
{
    public record MovieDto
    {
        [Required(ErrorMessage = "Title is required!")]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters long!")]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required!")]
        [AllowedMovieCategoryValues(typeof(MovieCategory))]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Poster url is required!")]
        [Url(ErrorMessage = "Invalid Url address!")]
        public string PosterUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Video url is required!")]
        [Url(ErrorMessage = "Invalid Url address!")]
        public string VideoUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Director is required!")]
        [MinLength(5, ErrorMessage = "Director must be at least 5 characters long!")]
        public string Director { get; set; } = string.Empty;

        [Required(ErrorMessage = "Release year is required!")]
        [ReleaseYear]
        public int ReleaseYear { get; set; } = 2025;
    }
}
