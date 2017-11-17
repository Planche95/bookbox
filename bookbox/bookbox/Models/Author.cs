using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookBox.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public List<Book> Books { get; set; }
    }
}
