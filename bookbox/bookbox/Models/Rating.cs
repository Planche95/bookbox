using Microsoft.AspNetCore.Identity;

namespace BookBox.Models
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
