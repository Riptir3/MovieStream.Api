using System.ComponentModel.DataAnnotations;

namespace MovieStream.Api.Models.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Not valid email address!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Passord is required!")]
        public string Password { get; set; } = string.Empty;
    }
}
