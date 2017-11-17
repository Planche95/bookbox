using BookBox.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookBox.ViewModels
{
    public class BookCreateEditViewModel
    {
        public int BookId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [RegularExpression(@"^(97(8|9))?\d{9}(\d|X)$", ErrorMessage = "Invalid ISBN number format")]
        public string ISBN { get; set; }

        public float AveragedRating { get; set; }

        public string PicturePath { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        public List<SelectListItem> Authors { get; set; }
    }
}
