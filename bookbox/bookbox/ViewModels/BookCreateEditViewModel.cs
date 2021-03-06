﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookBox.ViewModels
{
    public class BookCreateEditViewModel
    {
        public int BookId { get; set; }

        //The longest book title have 584 symbols
        [Required]
        [StringLength(584, MinimumLength = 1)]
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
        [StringLength(1000, MinimumLength = 200)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        public List<SelectListItem> Authors { get; set; }
    }
}
