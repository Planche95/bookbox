using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BookBox.Models;
using BookBox.ViewModels;
using Microsoft.Extensions.Logging;

namespace BookBox.Controllers
{
    public class SearchController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly ILogger _logger;

        public SearchController(IBookRepository bookRepository, IRatingRepository ratingRepository,
            ILogger<UserController> logger)
        {
            _bookRepository = bookRepository;
            _ratingRepository = ratingRepository;
            _logger = logger;
        }

        public IActionResult Index(string filter)
        {
            List<BookRatingViewModel> bookModels = new List<BookRatingViewModel>();
            IEnumerable<Book> books;

            if (string.IsNullOrEmpty(filter))
            {
                _logger.LogInformation(LoggingEvents.ListItems, "Getting all books for {USER}", User.Identity.Name);

                books = _bookRepository.Books;
            }
            else
            {
                _logger.LogInformation(LoggingEvents.ListItems, "Getting all fitered books for {USER}", User.Identity.Name);

                books = _bookRepository.Books
                    .Where(b => IsContainCaseInsensitive(b.Title, filter) ||
                                IsContainCaseInsensitive(b.Author.Name, filter) ||
                                IsContainCaseInsensitive(b.Author.LastName, filter) ||
                                b.ISBN.Contains(filter));
            }

            foreach (Book book in books)
            {
                bookModels.Add(new BookRatingViewModel()
                {
                    Book = book,
                    Rating = GetRatingValue(book.BookId)
                });
            }

            return View(bookModels);
        }

        //Simple Contains is case sensitive - Possible another solution: COLLATE on column in database
        bool IsContainCaseInsensitive(string baseString, string filter)
        {
            return baseString.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        private int GetRatingValue(int bookId)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("User"))
            {
                Rating rating = _ratingRepository.GetRatingByBookIdAndUserName(bookId, User.Identity.Name);

                if (rating != null)
                {
                    return rating.Value;
                }
            }

            return 0;
        }
    }
}