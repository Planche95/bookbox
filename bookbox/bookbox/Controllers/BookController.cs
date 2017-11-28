using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BookBox.Models;
using Microsoft.AspNetCore.Authorization;
using BookBox.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace BookBox.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger _logger;

        public BookController(IBookRepository bookRepository, IAuthorRepository authorRepository,
            ILogger<BookController> logger)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            Book book = _bookRepository.GetBookById(id);

            if (book == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound,
                        "Book with id {ID} not found", id);

                return new StatusCodeResult(404);
            }

            return View(book);
        }

        public IActionResult Create()
        {
            return View("CreateEdit",
                new BookCreateEditViewModel()
                {
                    ReleaseDate = DateTime.Now,
                    Authors = GetAuthorsSelectList()
                });
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

        public IActionResult Edit(int id)
        {
            Book book = _bookRepository.GetBookById(id);

            if (book == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound,
                        "Book with id {ID} not found", id);

                return new StatusCodeResult(404);
            }

            return View("CreateEdit",
                new BookCreateEditViewModel()
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

        [HttpPost]
        public IActionResult CreateEdit(BookCreateEditViewModel model)
        {
            int bookId;

            try
            {
                if (model.BookId == 0)
                {
                    _logger.LogInformation(LoggingEvents.InsertItem,
                        "Create Book with {BOOK}", model);

                    bookId = _bookRepository.CreateBook(
                    new Book()
                    {
                        Title = model.Title,
                        Description = model.Description,
                        ISBN = model.ISBN,
                        PicturePath = "/images/0747532699.jpg",
                        ReleaseDate = model.ReleaseDate,
                        AuthorId = model.AuthorId,
                        AveragedRating = 0
                    });
                }
                else
                {
                    _logger.LogInformation(LoggingEvents.UpdateItem,
                        "Edit Book {ID} with {BOOK}", model.BookId, model);

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
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggingEvents.CreateUpdateItemFailed, ex,
                        "Create/Update Book failed");

                ModelState.AddModelError("", "Something went wrong. Please try again");
                model.Authors = GetAuthorsSelectList();
                return View(model);
            }

            return RedirectToAction("Details", new { id = bookId });
        }

        public IActionResult Delete(int id)
        {
            Book book = _bookRepository.GetBookById(id);

            if (book == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound,
                        "Book with id {ID} not found", id);

                return new StatusCodeResult(404);
            }

            return View(book);
        }

        [HttpPost]
        public IActionResult Delete(Book book)
        {
            try
            {
                _logger.LogInformation(LoggingEvents.DeleteItem,
                        "Delete Book {ID}", book.BookId);

                _bookRepository.DeleteBook(book.BookId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggingEvents.DeleteItemFailed, ex,
                        "Delete Book failed");

                ModelState.AddModelError("", "Something went wrong. Please try again");
                return View(_bookRepository.GetBookById(book.BookId));
            }
            return RedirectToAction("Index", "Search");
        }
    }
}