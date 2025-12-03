using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieStream.Api.Models.DTOs;
using MovieStream.Api.Models.Entities;

namespace MovieStream.Api.Services
{

    public class MovieService
    {
        private readonly IMongoCollection<Movie> _movies;

        public MovieService(IOptions<MongoDbSettings> movieSettings, IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase(movieSettings.Value.DatabaseName);
            _movies = mongoDatabase.GetCollection<Movie>(movieSettings.Value.MoviesCollectionName);
        }

        public async Task<List<Movie>> GetAllAsync() =>
            await _movies.Find(_ => true).ToListAsync();

        public async Task<Movie?> GetByIdAsync(string id) =>
            await _movies.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Movie movie) =>
            await _movies.InsertOneAsync(movie);

        public async Task UpdateAsync(Movie updatedMovie) =>
            await _movies.ReplaceOneAsync(m => m.Id == updatedMovie.Id, updatedMovie);

        public async Task RemoveAsync(string id) =>
            await _movies.DeleteOneAsync(x => x.Id == id);
    }
}
