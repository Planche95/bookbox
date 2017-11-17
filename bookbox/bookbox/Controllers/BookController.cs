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
        public IActionResult Details(int id)
        {
            return View(_bookRepository.GetBookById(id));
        }

        public IActionResult Create()
        {
            return View("CreateEdit",
                new BookCreateEditViewModel()
                {
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

            if (model.BookId == 0)
            {
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

        public IActionResult Delete(int id)
        {
            return View(_bookRepository.GetBookById(id));
        }

        [HttpPost]
        public IActionResult Delete(Book book)
        {
            _bookRepository.DeleteBook(book.BookId);
            return RedirectToAction("Index", "Search");
        }
    }
}