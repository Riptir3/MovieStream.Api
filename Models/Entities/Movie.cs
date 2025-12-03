using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieStream.Api.Models.Entities
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public int ReleaseYear { get; set; } = 1900;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
