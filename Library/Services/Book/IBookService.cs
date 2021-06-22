using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Helpers;
using Library.Models.Book;

namespace Library.Services.Book
{
    public interface IBookService
    {
        Task<AOResult<IEnumerable<BookModel>>> GetBooksAsync(Func<BookModel, bool> func = null);

        Task<AOResult> AddBookAsync(BookModel reader);

        Task<AOResult> UpdateBookAsync(BookModel book, bool shouldSendUpdate = false);

        Task<AOResult<IEnumerable<string>>> GetCategoriesAsync();
    }
}
