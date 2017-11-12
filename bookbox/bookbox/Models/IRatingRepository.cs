using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBox.Models
{
    public interface IRatingRepository
    {
        IEnumerable<Rating> GetRatingsByUserId(string userId);
        IEnumerable<Rating> GetRatingsByBookId(int bookId);

        Rating GetRatingByBookIdAndUserId(int bookId, string userId);
        void CreateRating(Rating rating);
        void EditRating(Rating rating);
    }
}
