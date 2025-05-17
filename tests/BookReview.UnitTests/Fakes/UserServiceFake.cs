using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.UnitTests.Fakes;

public sealed class UserServiceFake : IUserService
{
    public Task<User> AddAsync(User entity)
    {
        return Task.FromResult(entity);
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<User>>(new List<User>());
    }

    public Task<User?> GetByIdAsync(long id)
    {
        var user = User.Create("sample", "sample@sample.com");
        user.Id = id;
        return Task.FromResult<User?>(user);
    }

    public Task<bool> LoginAsync(string username, string password, bool rememberMe)
    {
        return Task.FromResult(true);
    }

    public Task LogoutAsync()
    {
        return Task.CompletedTask;
    }

    public Task<bool> RegisterAsync(string username, string email, string password)
    {
        return Task.FromResult(true);
    }

    public Task<bool> RemoveAsync(User entity)
    {
        return Task.FromResult(true);
    }

    public Task<User> UpdateAsync(User entity)
    {
        return Task.FromResult(entity);
    }
}
