using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookBox.Models
{
    public class Book
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string ISBN { get; set; }

        public float AveragedRating { get; set; }

        public string PicturePath { get; set; }

        public string Description { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
