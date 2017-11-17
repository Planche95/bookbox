using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookBox.Models;
using BookBox.ViewModels;

namespace BookBox.Controllers
{
    public class SearchController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRatingRepository _ratingRepository;

        public SearchController(IBookRepository bookRepository, IRatingRepository ratingRepository)
        {
            _bookRepository = bookRepository;
            _ratingRepository = ratingRepository;
        }

        public IActionResult Index(string filter)
        {
            List<BookRatingViewModel> bookModels = new List<BookRatingViewModel>();
            IEnumerable<Book> books;

            if (string.IsNullOrEmpty(filter))
            {
                books = _bookRepository.Books;
            }
            else
            {
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