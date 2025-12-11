using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieStream.Api.Models.DTOs;
using MovieStream.Api.Models.Entities;

namespace MovieStream.Api.Services
{
    public class FavoriteService
    {
        private readonly IMongoCollection<Favorite> _favorites;

        public FavoriteService(IOptions<MongoDbSettings> mongoSettings, IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _favorites = mongoDatabase.GetCollection<Favorite>(mongoSettings.Value.FavoriteCollectionName);
        }

        public async Task<List<Favorite>> GetUserFavorites(string userId) =>
            await _favorites.Find(f => f.UserId == userId).ToListAsync();

        public async Task<bool> Exists(string userId, string movieId)
        {
            var fav = await _favorites.Find(f => f.UserId == userId && f.MovieId == movieId).FirstOrDefaultAsync();
            return fav != null;
        }

        public async Task<bool> AddFavorite(string userId, string movieId)
        {
            if (await Exists(userId, movieId)) return false;

            var fav = new Favorite()
            {
                UserId = userId,
                MovieId = movieId
            };
            await _favorites.InsertOneAsync(fav);
            return true;
        }

        public async Task<bool> RemoveFavorite(string userId, string movieId)
        {
            var deleteResult = await _favorites.DeleteOneAsync(f => f.UserId == userId && f.MovieId == movieId);
            return deleteResult.DeletedCount > 0;
        }
    }
}
