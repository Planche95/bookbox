using BookBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBox.ViewModels
{
    public class BookRatingViewModel
    {
        public Book Book { get; set; }
        public int Rating { get; set; }
    }
}
