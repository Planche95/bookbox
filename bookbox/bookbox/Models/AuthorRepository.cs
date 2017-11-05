using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBox.Models
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _appDbContext;

        public AuthorRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Author> Authors
        {
            get
            {
                return _appDbContext.Authors;
            }
        }

        public void CreateAuthor(Author author)
        {
            _appDbContext.Authors.Add(author);
            _appDbContext.SaveChanges();
        }

        public Author GetAuthorById(int authorId)
        {
            return _appDbContext.Authors.FirstOrDefault(a => a.AuthorId == authorId);
        }
    }
}
