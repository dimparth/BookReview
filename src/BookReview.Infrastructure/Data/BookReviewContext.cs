using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookReview.Infrastructure.Data;

public sealed class BookReviewContext : IdentityDbContext<User, IdentityRole<long>, long>, IUnitOfWork
{
    public BookReviewContext(DbContextOptions<BookReviewContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ReviewVote> ReviewVotes => Set<ReviewVote>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await base.SaveChangesAsync(cancellationToken);
}