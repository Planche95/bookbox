using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookBox.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BookBox.Controllers
{
    [Authorize(Roles = "User")]
    public class RatingController : Controller
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public RatingController(IRatingRepository ratingRepository, UserManager<IdentityUser> userManager)
        {
            _ratingRepository = ratingRepository;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult SaveRating(RatingJsonModel model)
        {
            IdentityUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            Rating rating = _ratingRepository.GetRatingByBookIdAndUserName(model.BookId, User.Identity.Name);

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
    }
}