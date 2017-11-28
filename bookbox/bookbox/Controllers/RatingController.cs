using System;
using Microsoft.AspNetCore.Mvc;
using BookBox.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace BookBox.Controllers
{
    [Authorize(Roles = "User")]
    public class RatingController : Controller
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger _logger;

        public RatingController(IRatingRepository ratingRepository, UserManager<IdentityUser> userManager,
            ILogger<RatingController> logger)
        {
            _ratingRepository = ratingRepository;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult SaveRating(RatingJsonModel model)
        {
            IdentityUser user = _userManager.GetUserAsync(HttpContext.User).Result;
            Rating rating = _ratingRepository.GetRatingByBookIdAndUserName(model.BookId, User.Identity.Name);

            try
            {
                if (rating == null)
                {
                    _logger.LogInformation(LoggingEvents.InsertItem,
                        "Create rating with value {VALUE} for book {BOOK} by user {USER}",
                        model.Rating, model.BookId, User.Identity.Name);

                    _ratingRepository.CreateRating(new Rating()
                    {
                        BookId = model.BookId,
                        UserId = user.Id,
                        Value = model.Rating
                    });
                }
                else
                {
                    _logger.LogInformation(LoggingEvents.InsertItem,
                        "Update rating {RATING} with value {VALUE} for book {BOOK} by user {USER}",
                        rating.RatingId, model.Rating, model.BookId, User.Identity.Name);

                    rating.Value = model.Rating;
                    _ratingRepository.EditRating(rating);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggingEvents.CreateUpdateItemFailed, ex,
                        "Create/Update rating failed");

                return Json(new { success = false, message = "Something went wrong. Please try again" });
            }

            return Json(new { success = true, message = "Rating Saved!"});
        }
    }
}