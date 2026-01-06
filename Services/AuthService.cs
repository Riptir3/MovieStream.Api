using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieStream.Api.Models.DTOs;
using MovieStream.Api.Models.Entities;
using TaskManager.Api.Models;

namespace MovieStream.Api.Services
{
    public class AuthService
    {
        private readonly IMongoCollection<User> _users;
        private readonly JwtService _jwtService;

        public AuthService(IOptions<MongoDbSettings> mongoSettings, IMongoClient mongoClient, JwtService jwtService)
        {
            var mongoDatabase = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _users = mongoDatabase.GetCollection<User>("Users");
            _jwtService = jwtService;
        }

        public async Task<Response<object>> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            var user = await _users.Find(u => u.Email == userRegisterDto.Email).FirstOrDefaultAsync(); 
            if(user != null)
            {
                return Response<object>.Fail("A user with this email already exists!");
            }

            PasswordService.CreatePasswordHash(userRegisterDto.Password, out string hash, out string salt);

            var newUser = new User
            {
                Username = userRegisterDto.Username,
                Email = userRegisterDto.Email,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            await _users.InsertOneAsync(newUser);

            return Response<object>.OK("User registered succesfully!");
        }

        public record LoginResponseDto(string Token);
        public async Task<Response<LoginResponseDto>> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _users.Find(u => u.Email == userLoginDto.Email).FirstOrDefaultAsync();
            if (user == null)
                return Response<LoginResponseDto>.Fail("Invalid Credentials!");

            var hash = PasswordService.VerifyPassword(userLoginDto.Password, user.PasswordHash, user.PasswordSalt);
            if (!hash)
                return Response<LoginResponseDto>.Fail("Invalid Credentials!");

            var token = _jwtService.GenerateToken(user);
            var data = new LoginResponseDto(token);
            return Response<LoginResponseDto>.OK("Login successfully!", data);
        }
    }
}
