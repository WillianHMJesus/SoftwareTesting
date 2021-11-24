using Api.Models;

namespace Api.Repositories
{
    public interface IUserRepository
    {
        User PasswordSignIn(string email, string password);
    }
}
