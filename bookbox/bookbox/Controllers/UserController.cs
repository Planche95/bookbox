using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookBox.ViewModels;
using BookBox.Models;
using Microsoft.AspNetCore.Identity;

namespace BookBox.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {

        private readonly IBookRepository _bookRepository;
        private readonly IRatingRepository _ratingRepository;

        public UserController(IBookRepository bookRepository, IRatingRepository ratingRepository)
        {
            _bookRepository = bookRepository;
            _ratingRepository = ratingRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Rating> ratings = _ratingRepository.GetRatingsByUserName(User.Identity.Name);
            List<BookRatingViewModel> bookModels = new List<BookRatingViewModel>();

            foreach (Rating rating in ratings)
            {
                bookModels.Add(new BookRatingViewModel()
                {
                    Book = _bookRepository.GetBookById(rating.BookId),
                    Rating = rating.Value
                });
            }

            return View(bookModels);
        }
    }
}