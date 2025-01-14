using System.Text.Json.Serialization;

namespace EQ_Internship.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }



}
