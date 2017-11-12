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
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(IBookRepository bookRepository, IRatingRepository ratingRepository
            , UserManager<IdentityUser> userManager)
        {
            _bookRepository = bookRepository;
            _ratingRepository = ratingRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            IdentityUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            IEnumerable<Rating> ratings = _ratingRepository.GetRatingsByUserId(user.Id);
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