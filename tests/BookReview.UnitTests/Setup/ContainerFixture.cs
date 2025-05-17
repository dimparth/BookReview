using BookReview.ApplicationCore.Interfaces;
using BookReview.ApplicationCore.Services;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.UnitTests.Setup;

public sealed class ContainerFixture
{
    public ServiceProvider ServiceProvider { get; private set; }

    public ContainerFixture()
    {
        var services = new ServiceCollection();

        // Substitute repositories
        var bookRepo = Substitute.For<IBookRepository>();
        var reviewRepo = Substitute.For<IReviewRepository>();
        var uow = Substitute.For<IUnitOfWork>();

        // Add Microsoft logger support
        services.AddLogging();

        // Inject substitutes
        services.AddSingleton(bookRepo);
        services.AddSingleton(reviewRepo);
        services.AddSingleton(uow);

        // Register real services
        services.AddTransient<IBookService, BookService>();
        services.AddTransient<IReviewService, ReviewService>();

        // Build service provider
        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        if (ServiceProvider is IDisposable disposable)
            disposable.Dispose();
    }
}