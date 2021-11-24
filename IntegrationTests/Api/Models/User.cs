using System.Collections.Generic;

namespace Api.Models
{
    public class User
    {
        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; }

        public static List<User> Users = new List<User>
        {
            new User("test@test.com", "Test@123")
        };
    }
}
