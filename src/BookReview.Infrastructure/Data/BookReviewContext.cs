using BookReview.ApplicationCore.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookReview.Infrastructure.Data;

public sealed class BookReviewContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    public BookReviewContext(DbContextOptions<BookReviewContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ReviewVote> ReviewVotes => Set<ReviewVote>();

}