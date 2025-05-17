using BookReview.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BookReview.Web.Extensions;

public static class ContextExtensions
{
    public static async Task InitializeDbContextAndSeedData(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var provider = scope.ServiceProvider;

        try
        {
            var db = provider.GetRequiredService<BookReviewContext>();
            var dbConn = db.Database.GetDbConnection();

            if (dbConn is SqliteConnection sqlite && !File.Exists(sqlite.DataSource))
            {
                await db.Database.MigrateAsync();
            }
            await provider.SeedDataAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Migration/Seeding failed: {ex.Message}");
        }
    }
}
