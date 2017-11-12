using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookBox.Models;
using Microsoft.AspNetCore.Authorization;
using BookBox.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace BookBox.Controllers
{
    
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public BookController(IBookRepository bookRepository, IAuthorRepository authorRepository
            , IRatingRepository ratingRepository, UserManager<IdentityUser> userManager)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _ratingRepository = ratingRepository;
            _userManager = userManager;
        }

        private bool IsLoggedAndInRole()
        {
            return User.Identity.IsAuthenticated && User.IsInRole("User");
        }

        private int GetRatingValue(int bookId)
        {
            if (IsLoggedAndInRole())
            {
                IdentityUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                Rating rating = _ratingRepository.GetRatingByBookIdAndUserId(bookId, user.Id);

                if (rating != null)
                {
                    return rating.Value;
                }
            }

            return 0;
        }

 
        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult SaveRating(RatingJsonModel model)
        {
            IdentityUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            Rating rating = _ratingRepository.GetRatingByBookIdAndUserId(model.BookId, user.Id);

            if (rating == null)
            {
                _ratingRepository.CreateRating(new Rating()
                {
                    BookId = model.BookId,
                    UserId = user.Id,
                    Value = model.Rating
                });
            }
            else
            {
                rating.Value = model.Rating;
                _ratingRepository.EditRating(rating);
            }

            return Json(new { });
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

        public IActionResult Details(int id)
        {
            return View(_bookRepository.GetBookById(id));
        }

        private List<SelectListItem> GetAuthorsSelectList()
        {
            List<SelectListItem> authors = new List<SelectListItem>();

            foreach (Author author in _authorRepository.Authors)
            {
                authors.Add(new SelectListItem()
                {
                    Text = author.Name + " " + author.LastName,
                    Value = author.AuthorId.ToString()
                });
            }

            return authors;

        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View("CreateEdit",
                new BookCreateViewModel()
                {
                    Authors = GetAuthorsSelectList()
                });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateEdit(BookCreateViewModel model)
        {
            int bookId;

            if (model.BookId == 0)
            {
                bookId = _bookRepository.CreateBook(
                new Book()
                {
                    Title = model.Title,
                    Description = model.Description,
                    ISBN = model.ISBN,
                    PicturePath = model.PicturePath,
                    ReleaseDate = model.ReleaseDate,
                    AuthorId = model.AuthorId,
                    AveragedRating = 0
                });
            }
            else
            {
                bookId = model.BookId;
                _bookRepository.EditBook(
                new Book()
                {
                    BookId = model.BookId,
                    Title = model.Title,
                    Description = model.Description,
                    ISBN = model.ISBN,
                    PicturePath = model.PicturePath,
                    ReleaseDate = model.ReleaseDate,
                    AuthorId = model.AuthorId,
                    AveragedRating = model.AveragedRating
                });
            }

            return RedirectToAction("Details", new { id = bookId });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Book book = _bookRepository.GetBookById(id);

            return View("CreateEdit",
                new BookCreateViewModel()
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Description = book.Description,
                    ISBN = book.ISBN,
                    PicturePath = book.PicturePath,
                    ReleaseDate = book.ReleaseDate,
                    AuthorId = book.AuthorId,
                    AveragedRating = book.AveragedRating,
                    Authors = GetAuthorsSelectList()
                });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            return View(_bookRepository.GetBookById(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Book book)
        {
            _bookRepository.DeleteBook(book.BookId);

            return RedirectToAction("Index");
        }
    }
}