using Backend.Entities;

namespace Backend.Models
{
    public class AuthenticateResponse
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public long TokenExpires { get; set; }

        public AuthenticateResponse(User user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
        }
    }

    public class RefreshResponse
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public long TokenExpires { get; set; }

        public RefreshResponse(User user)
        {
            UserId = user.UserId;
        }
    }
}