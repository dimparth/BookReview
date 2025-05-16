using Microsoft.AspNetCore.Identity;

namespace BookReview.ApplicationCore.Domain;

public sealed class User : IdentityUser<long>
{
    private User() { }

    public static User Create(string username, string email)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required.");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.");

        return new User
        {
            UserName = username,
            Email = email
        };
    }
}
