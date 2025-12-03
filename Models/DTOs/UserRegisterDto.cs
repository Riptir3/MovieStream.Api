using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieStream.Api.Models.DTOs
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Username is required!")]
        [MinLength(5,ErrorMessage ="Username must be at least 5 characters long!")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage ="Email is required!")]
        [EmailAddress(ErrorMessage ="Not valid email address!")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage ="Passord is required!")]
        [MinLength(7, ErrorMessage = "Password must be at least 7 characters long.")]
        public string Password { get; set; } = null!;

        [AllowedValues(["User"])]
        public string Role { get; set;} = "User";
    }
}
