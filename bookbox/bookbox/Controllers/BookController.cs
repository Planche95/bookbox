using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookBox.Models;

namespace BookBox.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
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
                    .Where(b => isContainCaseInsensitive(b.Title, filter) ||
                                isContainCaseInsensitive(b.Author.Name, filter) ||
                                isContainCaseInsensitive(b.Author.LastName, filter) ||
                                b.ISBN.Contains(filter));
            }

            return View(books);
        }

        //Simple Contains is case sensitive - Possible another solution: COLLATE on column in database
        bool isContainCaseInsensitive(string baseString, string filter)
        {
            return baseString.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        public IActionResult Details(int id)
        {
            return View(_bookRepository.GetBookById(id));
        }
    }
}