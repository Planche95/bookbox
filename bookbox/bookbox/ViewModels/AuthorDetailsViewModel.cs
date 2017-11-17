using BookBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBox.ViewModels
{
    public class AuthorDetailsViewModel
    {
        public Author Author { get; set; }
        public List<int> Ratings { get; set; }
    }
}
