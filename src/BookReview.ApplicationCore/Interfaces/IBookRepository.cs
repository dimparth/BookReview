﻿using BookReview.ApplicationCore.Domain;

namespace BookReview.ApplicationCore.Interfaces;

public interface IBookRepository : IRepository<Book, long>
{
    Task<Book?> GetBookAndReviewsByIdAsync(long bookId);
    Task<IList<Book>> GetBooksByFilterAsync(BookFilter filter);
}
