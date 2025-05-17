using BookReview.ApplicationCore.Domain;

namespace BookReview.ApplicationCore.Interfaces;

public interface IUserService : IService<User, long>
{
    Task<bool> RegisterAsync(string username, string email, string password);
    Task<bool> LoginAsync(string username, string password, bool rememberMe);
    Task LogoutAsync();
}
