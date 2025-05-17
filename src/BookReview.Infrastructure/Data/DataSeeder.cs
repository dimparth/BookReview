using BookReview.ApplicationCore.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookReview.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedDataAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BookReviewContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        
        // Seed Users (only if not exist)
        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                User.Create("alice", "alice@example.com"),
                User.Create("bob", "bob@example.com")
            };

            foreach (var user in users)
            {
                var exists = await userManager.FindByNameAsync(user.UserName ?? "");
                if (exists == null)
                    await userManager.CreateAsync(user, "Password123!");
            }

            await context.SaveChangesAsync();
        }

        // Seed Books
        if (!context.Books.Any())
        {
            var books = new List<Book>
            {
                Book.Create("The Pragmatic Programmer", "Andrew Hunt", 2019, "Programming"),
                Book.Create("Clean Code", "Robert C. Martin", 2005, "Programming"),
                Book.Create("Domain-Driven Design", "Eric Evans", 2018, "Architecture")
            };

            await context.Books.AddRangeAsync(books);
            await context.SaveChangesAsync();
        }

        // Seed Reviews
        if (!context.Reviews.Any())
        {
            var dbUsers = context.Users.ToList();
            var dbBooks = context.Books.ToList();

            var reviews = new List<Review>
            {
                Review.Create("Excellent book for devs.", 5, dbUsers[0].Id, dbBooks[0].Id),
                Review.Create("Great insights into code quality.", 4, dbUsers[1].Id, dbBooks[1].Id),
                Review.Create("A bit dry but useful.", 3, dbUsers[0].Id, dbBooks[2].Id)
            };

            await context.Reviews.AddRangeAsync(reviews);
            await context.SaveChangesAsync();
        }

        // Seed ReviewVotes
        if (!context.ReviewVotes.Any())
        {
            var dbUsers = context.Users.ToList();
            var dbReviews = context.Reviews.ToList();

            var votes = new List<ReviewVote>
            {
                ReviewVote.Create(dbUsers[1].Id, dbReviews[0].Id, true),
                ReviewVote.Create(dbUsers[0].Id, dbReviews[1].Id, false),
                ReviewVote.Create(dbUsers[1].Id, dbReviews[2].Id, true)
            };

            await context.ReviewVotes.AddRangeAsync(votes);
            await context.SaveChangesAsync();
        }
    }
}
