using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieStream.Api.Models.DTOs;
using MovieStream.Api.Models.Entities;

namespace MovieStream.Api.Services
{
    public class MovieReportService
    {
        private readonly IMongoCollection<MovieReport> _movieReports;

        public MovieReportService(IOptions<MongoDbSettings> mongoSettings, IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _movieReports = mongoDatabase.GetCollection<MovieReport>(mongoSettings.Value.MovieReportCollectionName);
        }
        public async Task<List<MovieReport>> GetAllAsync()
        {
            return await _movieReports.Find(_ => true).ToListAsync();
        }

        public async Task CreateAsync(MovieReport report) =>
            await _movieReports.InsertOneAsync(report);
    }
}
