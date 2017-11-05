using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookbox.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int Value { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
