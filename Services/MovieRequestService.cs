using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieStream.Api.Models.DTOs;
using MovieStream.Api.Models.Entities;

namespace MovieStream.Api.Services
{
    public class MovieRequestService
    {
        private readonly IMongoCollection<MovieRequest> _movieRequests;

        public MovieRequestService(IOptions<MongoDbSettings> mongoSettings, IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _movieRequests = mongoDatabase.GetCollection<MovieRequest>(mongoSettings.Value.MovieRequestCollectionName);
        }
        public async Task<List<MovieRequest>> GetAllAsync()
        {
            return await _movieRequests.Find(r => r.Status == "Active").ToListAsync();
        }

        public async Task<MovieRequest> FindById(string id) =>
            await _movieRequests.Find(r => r.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(MovieRequest requestedMovie) =>
            await _movieRequests.ReplaceOneAsync(m => m.Id == requestedMovie.Id, requestedMovie);

        public async Task CreateAsync(MovieRequest movie) =>
            await _movieRequests.InsertOneAsync(movie);
    }
}
