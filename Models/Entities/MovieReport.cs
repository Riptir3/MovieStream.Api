using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieStream.Api.Models.Entities
{
    public class MovieReport
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string MovieId { get; set; } = null!;
        public string? Comment { get; set; }
        public string UserId { get; set; } = null!;
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
