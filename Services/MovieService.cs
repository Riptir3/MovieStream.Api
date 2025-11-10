using MovieStream.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieStream.Api.Models.Entities;
using MovieStream.Api.Models.DTOs;

namespace MovieStream.Api.Services
{

    public class MovieService
    {
        private readonly IMongoCollection<Movie> _movies;

        public MovieService(IOptions<MongoDbSettings> movieSettings)
        {
            var mongoClient = new MongoClient(movieSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(movieSettings.Value.DatabaseName);
            _movies = mongoDatabase.GetCollection<Movie>(movieSettings.Value.MoviesCollectionName);
        }

        public async Task<List<Movie>> GetAllAsync() =>
           await _movies.Find(_ => true).ToListAsync();

        public async Task<Movie?> GetAsync(string id) =>
            await _movies.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Movie movie) =>
            await _movies.InsertOneAsync(movie);

        public async Task RemoveAsync(string id) =>
            await _movies.DeleteOneAsync(x => x.Id == id);
    }
}
