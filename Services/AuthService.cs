using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieStream.Api.Models.DTOs;
using MovieStream.Api.Models.Entities;

namespace MovieStream.Api.Services
{
    public class AuthService
    {
        private readonly IMongoCollection<User> _users;
        private readonly JwtService _jwtService;
        private readonly PasswordService _passwordService = new PasswordService();

        public AuthService(IOptions<MongoDbSettings> mongoSettings, IMongoClient mongoClient, JwtService jwtService)
        {
            var mongoDatabase = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _users = mongoDatabase.GetCollection<User>("Users");
            _jwtService = jwtService;
        }

        public async Task<AuthResult?> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            var user = await _users.Find(u => u.Email == userRegisterDto.Email).FirstOrDefaultAsync();
            if(user != null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "A user with this email already exists!"
                };
            }

            _passwordService.CreatePasswordHash(userRegisterDto.Password, out string hash, out string salt);

            var newUser = new User
            {
                Username = userRegisterDto.Username,
                Email = userRegisterDto.Email,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            await _users.InsertOneAsync(newUser);

            return new AuthResult
            {
                Success = true,
                Message = "User registered succesfully!",
                User = newUser
            };
        }

        public async Task<AuthResult?> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _users.Find(u => u.Email == userLoginDto.Email).FirstOrDefaultAsync();
            if (user == null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "User not found!"
                };
            }

            var hash = _passwordService.VerifyPassword(userLoginDto.Password, user.PasswordHash, user.PasswordSalt);

            if (!hash)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Invalid Credentials!"
                };
            }

            var token = _jwtService.GenerateToken(user);

            return new AuthResult
            {
                Success = true,
                token = token
            };
        }
    }
}
