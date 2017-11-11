using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookBox.Models;

namespace BookBox.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            return View(_authorRepository.GetAuthorById(id));
        }

        public IActionResult Create()
        {
            return View("CreateEdit");
        }

        [HttpPost]
        public IActionResult Create(Author model)
        {
            int authorId =_authorRepository.CreateAuthor(model);

            return RedirectToAction("Details", new { id = authorId });
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

        public IActionResult Edit(int id)
        {
            return View("CreateEdit", _authorRepository.GetAuthorById(id));
        }

        public IActionResult Delete(int id)
        {
            return View(_authorRepository.GetAuthorById(id));
        }

        [HttpPost]
        public IActionResult Delete(Author author)
        {
            _authorRepository.DeleteAuthor(author.AuthorId);

            return RedirectToAction("Index", "Book");
        }
    }
}