using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookBox.Models;
using BookBox.ViewModels;
using Microsoft.Extensions.Logging;

namespace BookBox.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly ILogger _logger;

        public AuthorController(IAuthorRepository authorRepository, IRatingRepository ratingRepository,
            ILogger<AuthorController> logger)
        {
            _authorRepository = authorRepository;
            _ratingRepository = ratingRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_authorRepository.Authors);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            Author author = _authorRepository.GetAuthorById(id);

            if (author == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound,
                        "Author with id {ID} not found", id);

                return new StatusCodeResult(404);
            }

            List<int> ratings = new List<int>();
            author.Books.ForEach(b => ratings.Add(GetRatingValue(b.BookId)));

            return View(new AuthorDetailsViewModel()
                {
                    Author = _authorRepository.GetAuthorById(id),
                    Ratings = ratings
                });
        }

        private int GetRatingValue(int bookId)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("User"))
            {
                Rating rating = _ratingRepository.GetRatingByBookIdAndUserName(bookId, User.Identity.Name);

                if (rating != null)
                {
                    return rating.Value;
                }
            }

            return 0;
        }

        public IActionResult Create()
        {
            return View("CreateEdit");
        }

        public IActionResult Edit(int id)
        {
            Author author = _authorRepository.GetAuthorById(id);

            if (author == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound,
                        "Author with id {ID} not found", id);

                return new StatusCodeResult(404);
            }

            return View("CreateEdit", author);
        }

        [HttpPost]
        public IActionResult CreateEdit(Author model)
        {
            int authorId;

            try
            {
                if (model.AuthorId == 0)
                {
                    _logger.LogInformation(LoggingEvents.InsertItem,
                        "Create Author with {AUTHOR}", model);

                    authorId = _authorRepository.CreateAuthor(model);
                }
                else
                {
                    _logger.LogInformation(LoggingEvents.UpdateItem,
                        "Edit Author {ID} with {AUTHOR}", model.AuthorId, model);

                    authorId = model.AuthorId;
                    _authorRepository.EditAuthor(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggingEvents.CreateUpdateItemFailed, ex,
                        "Create/Update Author failed");

                ModelState.AddModelError("", "Something went wrong. Please try again");
                return View(model);
            }

            return RedirectToAction("Details", new { id = authorId });
        }

        public IActionResult Delete(int id)
        {
            Author author = _authorRepository.GetAuthorById(id);

            if (author == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound,
                        "Author with id {ID} not found", id);

                return new StatusCodeResult(404);
            }

            return View(author);
        }

        [HttpPost]
        public IActionResult Delete(Author author)
        {
            try
            {
                _logger.LogInformation(LoggingEvents.DeleteItem,
                        "Delete Author {ID}", author.AuthorId);

                _authorRepository.DeleteAuthor(author.AuthorId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LoggingEvents.DeleteItemFailed, ex,
                        "Delete Author failed");

                ModelState.AddModelError("", "Something went wrong. Please try again");
                return View(_authorRepository.GetAuthorById(author.AuthorId));
            }

            return RedirectToAction("Index", "Search");
        }
    }
}