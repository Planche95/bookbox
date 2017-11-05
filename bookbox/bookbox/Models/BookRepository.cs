﻿using System;
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
                return _appDbContext.Books;
            }
        }

        public void CreateBook(Book book)
        {
            _appDbContext.Books.Add(book);
            _appDbContext.SaveChanges();
        }

        public Book GetBookById(int bookId)
        {
            return _appDbContext.Books.FirstOrDefault(b => b.BookId == bookId);
        }
    }
}
