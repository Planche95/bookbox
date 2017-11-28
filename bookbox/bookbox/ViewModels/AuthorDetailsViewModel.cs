using BookBox.Models;
using System.Collections.Generic;

namespace BookBox.ViewModels
{
    public class AuthorDetailsViewModel
    {
        public Author Author { get; set; }
        public List<int> Ratings { get; set; }
    }
}
