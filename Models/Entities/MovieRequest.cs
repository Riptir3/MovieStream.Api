using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieStream.Api.Models.Entities
{
    public class MovieRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Director { get; set; } = null!;
        public int ReleaseYear { get; set; }
        public string? Comment { get; set; }
        public string UserId { get; set; } = null!;
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
