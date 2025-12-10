using System.ComponentModel.DataAnnotations;

namespace MovieStream.Api.Models.DTOs
{
    public class MovieReportDto
    {
        [Required]
        public string MovieId { get; set; }
        
        [Required(ErrorMessage = "Comment is required!")]
        [MaxLength(100,ErrorMessage = "Max length of comment is 100 characters!")]
        public string Comment { get; set; }
    }
}
