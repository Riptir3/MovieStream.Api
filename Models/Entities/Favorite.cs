using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieStream.Api.Models.Entities
{
    public class Favorite
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }
        public string MovieId { get; set; }
    }
}
