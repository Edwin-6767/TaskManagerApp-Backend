using Dapper;
using EQ_Internship.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace EQ_Internship.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDbConnection _dbConnection;
        private readonly IConfiguration _configuration;

        public AuthService(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _configuration = configuration;
        }

        // Register User
        public async Task<string> RegisterUserAsync(string username, string email, string password)
        {
            var existingUser = await _dbConnection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Email = @Email", new { email });

            if (existingUser != null)
                return "User already exists!";

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var sql = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";
            var result = await _dbConnection.ExecuteAsync(sql, new { username, email, Password = hashedPassword });

            return result > 0 ? "Registration successful!" : "Failed to register user.";
        }

        // Login User
        public async Task<LoginResponse> LoginUserAsync(string username, string password)
        {
            var user = await _dbConnection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Username = @Username", new { username });

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null; // You can return null or handle the error as needed

            var token = GenerateJwtToken(user);

            return new LoginResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                Token = token
            };
        }


        // Helper function to generate JWT Token
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


}
