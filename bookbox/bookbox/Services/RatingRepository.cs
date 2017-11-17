using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<Rating> GetRatingsByUserName(string userName)
        {
            return _appDbContext.Ratings
                .Include(r => r.User)
                .Where(r => r.User.UserName.Equals(userName));
        }

        public Rating GetRatingByBookIdAndUserName(int bookId, string userName)
        {
            return _appDbContext.Ratings
                .Where(r => r.BookId == bookId && r.User.UserName.Equals(userName))
                .Include(r => r.User)
                .FirstOrDefault();
        }
    }
}
