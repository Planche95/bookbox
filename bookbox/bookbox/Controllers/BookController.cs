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
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookController(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

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

        public IActionResult Details(int id)
        {
            return View(_bookRepository.GetBookById(id));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
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

            return View(new BookCreateViewModel()
            {
                Authors = authors
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(BookCreateViewModel model)
        {
            int bookId = _bookRepository.CreateBook(
                new Book()
                {
                    Title = model.Title,
                    Description = model.Description,
                    ISBN = model.ISBN,
                    PicturePath = model.PicturePath,
                    ReleaseDate = model.ReleaseDate,
                    AuthorId = model.AuthorId,
                    AveragedRating = 0
                }
            );

            return RedirectToAction("Details", new { id = bookId });
        }
    }
}