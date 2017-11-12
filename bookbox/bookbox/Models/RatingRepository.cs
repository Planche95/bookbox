using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBox.Models
{
    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _appDbContext;

        public RatingRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void CreateRating(Rating rating)
        {
            Rating newRating = new Rating()
            {
                BookId = rating.BookId,
                UserId = rating.UserId,
                Value = rating.Value
            };
            _appDbContext.Add(newRating);
            _appDbContext.SaveChanges();
        }

        public void EditRating(Rating rating)
        {
            Rating existingRating = _appDbContext.Ratings
                .FirstOrDefault(r => r.BookId == rating.BookId && r.UserId.Equals(rating.UserId));

            existingRating.Value = rating.Value;
            _appDbContext.SaveChanges();
        }

        public IEnumerable<Rating> GetRatingsByBookId(int bookId)
        {
            return _appDbContext.Ratings.Where(r => r.BookId == bookId);
        }

        public IEnumerable<Rating> GetRatingsByUserId(string userId)
        {
            return _appDbContext.Ratings.Where(r => r.UserId.Equals(userId));
        }

        public Rating GetRatingByBookIdAndUserId(int bookId, string userId)
        {
            return _appDbContext.Ratings.FirstOrDefault(r => r.BookId == bookId && r.UserId.Equals(userId));
        }
    }
}
