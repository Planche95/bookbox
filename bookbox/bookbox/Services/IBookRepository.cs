using System.Collections.Generic;

namespace BookBox.Models
{
    public interface IBookRepository
    {
        IEnumerable<Book> Books { get; }

        Book GetBookById(int bookId);
        int CreateBook(Book book);
        void EditBook(Book book);
        void DeleteBook(int id);

    }
}
