using Microsoft.EntityFrameworkCore;
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

        public int CreateAuthor(Author author)
        {
            _appDbContext.Authors.Add(author);
            _appDbContext.SaveChanges();

            return author.AuthorId;
        }

        public Author GetAuthorById(int authorId)
        {
            return _appDbContext.Authors
                .Include(a => a.Books)
                .FirstOrDefault(a => a.AuthorId == authorId);
        }

        public void EditAuthor(Author author)
        {
            Author editedAuthor = _appDbContext.Authors.First(a => a.AuthorId == author.AuthorId);

            editedAuthor.Name = author.Name;
            editedAuthor.LastName = author.LastName;

            _appDbContext.SaveChanges();
        }

        public void DeleteAuthor(int id)
        {
            Author author = _appDbContext.Authors.First(a => a.AuthorId == id);
            _appDbContext.Authors.Remove(author);
            _appDbContext.SaveChanges();
        }
    }
}
