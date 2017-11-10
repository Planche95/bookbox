using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBox.Models
{
    public interface IBookRepository
    {
        IEnumerable<Book> Books { get; }

        Book GetBookById(int bookId);
        int CreateBook(Book book);
    }
}
