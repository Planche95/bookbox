using System.Collections.Generic;

namespace BookBox.Models
{
    public interface IAuthorRepository
    {
        IEnumerable<Author> Authors { get; }

        Author GetAuthorById(int authorId);
        int CreateAuthor(Author author);
        void EditAuthor(Author author);
        void DeleteAuthor(int id);
    }
}
