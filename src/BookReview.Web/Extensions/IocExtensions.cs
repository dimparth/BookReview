using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using BookReview.ApplicationCore.Services;
using BookReview.Infrastructure.Data;
using BookReview.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookReview.Web.Extensions;

public static class IocExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<BookReviewContext>(options =>
        options.UseSqlite(config.GetConnectionString("DefaultConnection")));
        services.AddIdentity<User, IdentityRole<long>>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
        })
            .AddEntityFrameworkStores<BookReviewContext>()
            .AddDefaultTokenProviders();
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }

}
