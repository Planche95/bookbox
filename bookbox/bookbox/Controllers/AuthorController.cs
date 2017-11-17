using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookBox.Models;
using BookBox.ViewModels;

namespace BookBox.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IRatingRepository _ratingRepository;

        public AuthorController(IAuthorRepository authorRepository, IRatingRepository ratingRepository)
        {
            _authorRepository = authorRepository;
            _ratingRepository = ratingRepository;
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            List<int> ratings = new List<int>();

            _authorRepository.GetAuthorById(id).Books
                .ForEach(b => ratings.Add(GetRatingValue(b.BookId)));

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
            return View("CreateEdit", _authorRepository.GetAuthorById(id));
        }

        [HttpPost]
        public IActionResult CreateEdit(Author model)
        {
            int authorId;

            if (model.AuthorId == 0)
            {
                authorId = _authorRepository.CreateAuthor(model);
            }
            else
            {
                authorId = model.AuthorId;
                _authorRepository.EditAuthor(model);
            }

            return RedirectToAction("Details", new { id = authorId });
        }

        public IActionResult Delete(int id)
        {
            return View(_authorRepository.GetAuthorById(id));
        }

        [HttpPost]
        public IActionResult Delete(Author author)
        {
            _authorRepository.DeleteAuthor(author.AuthorId);

            return RedirectToAction("Index", "Search");
        }
    }
}