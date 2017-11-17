using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBox.Models
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _appDbContext;

        public BookRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Book> Books
        {
            get
            {
                return _appDbContext.Books
                    .Include(b => b.Author);
            }
        }

        public int CreateBook(Book book)
        {
            _appDbContext.Books.Add(book);
            _appDbContext.SaveChanges();

            return book.BookId;
        }

        public void EditBook(Book book)
        {
            Book editedBook = _appDbContext.Books.First(b => b.BookId == book.BookId);

            editedBook.PicturePath = book.PicturePath;
            editedBook.Description = book.Description;
            editedBook.ISBN = book.ISBN;
            editedBook.ReleaseDate = book.ReleaseDate;
            editedBook.Title = book.Title;
            editedBook.AuthorId = book.AuthorId;

            _appDbContext.SaveChanges();
        }

        public void DeleteBook(int id)
        {
            Book book =_appDbContext.Books.First(b => b.BookId == id);
            _appDbContext.Books.Remove(book);
            _appDbContext.SaveChanges();
        }

        public Book GetBookById(int bookId)
        {
            return _appDbContext.Books
                .Include(b => b.Author)
                .FirstOrDefault(b => b.BookId == bookId);
        }
    }
}
