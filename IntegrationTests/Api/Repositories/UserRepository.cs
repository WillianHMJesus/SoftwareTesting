using Api.Models;
using System.Linq;

namespace Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User PasswordSignIn(string email, string password)
        {
            return User.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
        }
    }
}
