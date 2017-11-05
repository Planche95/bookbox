using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBox.Models
{
    public interface IAuthorRepository
    {
        IEnumerable<Author> Authors { get; }

        Author GetAuthorById(int authorId);
        void CreateAuthor(Author author);
    }
}
