using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookBox.ViewModels;
using BookBox.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BookBox.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {

        private readonly IBookRepository _bookRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly ILogger _logger;

        public UserController(IBookRepository bookRepository, IRatingRepository ratingRepository,
            ILogger<UserController> logger)
        {
            _bookRepository = bookRepository;
            _ratingRepository = ratingRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation(LoggingEvents.ListItems, "Getting all rated books by {USER}", User.Identity.Name);

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