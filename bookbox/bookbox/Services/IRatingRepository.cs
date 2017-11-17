using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBox.Models
{
    public interface IRatingRepository
    {
        IEnumerable<Rating> GetRatingsByUserName(string userName);
        IEnumerable<Rating> GetRatingsByBookId(int bookId);

        Rating GetRatingByBookIdAndUserName(int bookId, string userName);
        void CreateRating(Rating rating);
        void EditRating(Rating rating);
    }
}
