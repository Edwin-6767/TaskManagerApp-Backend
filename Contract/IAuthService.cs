using EQ_Internship.Models;
using Microsoft.AspNetCore.Identity;

public interface IAuthService
{
    Task<string> RegisterUserAsync(string username, string email, string password);
    Task<LoginResponse> LoginUserAsync(string username, string password);
}

