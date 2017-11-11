using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookBox.Models;
using Microsoft.AspNetCore.Authorization;
using BookBox.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookBox.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookController(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        [AllowAnonymous]
        public IActionResult Index(string filter)
        {
            IEnumerable<Book> books = _bookRepository.Books;

            if (String.IsNullOrEmpty(filter))
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

            return View(books);
        }

        //Simple Contains is case sensitive - Possible another solution: COLLATE on column in database
        bool IsContainCaseInsensitive(string baseString, string filter)
        {
            return baseString.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        [AllowAnonymous]
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

        public IActionResult Create()
        {
            return View("CreateEdit",
                new BookCreateViewModel()
                {
                    Authors = GetAuthorsSelectList()
                });
        }

        [HttpPost]
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

        public IActionResult Delete(int id)
        {
            return View(_bookRepository.GetBookById(id));
        }

        [HttpPost]
        public IActionResult Delete(Book book)
        {
            _bookRepository.DeleteBook(book.BookId);

            return RedirectToAction("Index");
        }
    }
}