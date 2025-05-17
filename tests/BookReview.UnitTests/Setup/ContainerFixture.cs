using BookReview.ApplicationCore.Domain;
using BookReview.ApplicationCore.Interfaces;
using BookReview.ApplicationCore.Services;
using BookReview.UnitTests.Fakes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace BookReview.UnitTests.Setup;

public sealed class ContainerFixture
{
    public ServiceProvider ServiceProvider { get; private set; }

    public ContainerFixture()
    {
        var services = new ServiceCollection();

        var bookRepo = Substitute.For<IBookRepository>();
        var reviewRepo = Substitute.For<IReviewRepository>();
        var uow = Substitute.For<IUnitOfWork>();

        services.AddLogging();

        services.AddSingleton(bookRepo);
        services.AddSingleton(reviewRepo);
        services.AddSingleton(uow);

        services.AddTransient<IBookService, BookService>();
        services.AddTransient<IReviewService, ReviewService>();
        services.AddTransient<IUserService, UserServiceFake>();

        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        if (ServiceProvider is IDisposable disposable)
            disposable.Dispose();
    }
}

[CollectionDefinition("ServiceCollection")]
public class TestServiceCollection : ICollectionFixture<ContainerFixture> { }