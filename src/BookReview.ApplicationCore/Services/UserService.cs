using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookReview.ApplicationCore.Services;

public sealed class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public async Task<User> AddAsync(User entity)
    {
        var result = await _userManager.CreateAsync(entity);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return entity;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> RemoveAsync(User entity)
    {
        var result = await _userManager.DeleteAsync(entity);
        return result.Succeeded;
    }

    public async Task<User> UpdateAsync(User entity)
    {
        var result = await _userManager.UpdateAsync(entity);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"User update failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return entity;
    }

    public async Task<bool> RegisterAsync(string username, string email, string password)
    {
        var user = User.Create(username, email);
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"User registration failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        return result.Succeeded;
    }

    public async Task<bool> LoginAsync(string username, string password, bool rememberMe)
    {
        var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);
        return result.Succeeded;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
