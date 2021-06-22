using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Events;
using Library.Helpers;
using Library.Models.Book;
using Library.Services.Rest;
using Prism.Events;

namespace Library.Services.Book
{
    public class BookService : IBookService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRestService _restService;

        private TaskCompletionSource<bool> _loadBooksCompletionSource = new TaskCompletionSource<bool>();

        private string BASE_URL = "https://api-library.azurewebsites.net/api/library/books";
        private ICollection<BookModel> _books;

        public BookService(
            IEventAggregator eventAggregator,
            IRestService restService)
        {
            _eventAggregator = eventAggregator;
            _restService = restService;
        }

        #region -- IReaderService implementation --

        public async Task<AOResult<IEnumerable<BookModel>>> GetBooksAsync(Func<BookModel, bool> func = null)
        {
            var result = new AOResult<IEnumerable<BookModel>>();

            try
            {
                await Task.Run(LoadBooksAsync).ConfigureAwait(false);

                var tcsResult = await _loadBooksCompletionSource.Task;

                if (tcsResult)
                {
                    var books = func == null
                        ? _books
                        : _books.Where(func);

                    result.SetSuccess(books);
                }
                else
                {
                    result.SetFailure();

                    RunLoadBooksTask();
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(GetBooksAsync)}: exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult> AddBookAsync(BookModel book)
        {
            var result = new AOResult();

            try
            {
                BookModelDTO bookDTO = book;

                await _restService.PostAsync<BookModelDTO>(BASE_URL, bookDTO);

                result.SetSuccess();

                SendUpdateEvent();
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(AddBookAsync)}: exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult> UpdateBookAsync(BookModel book, bool shouldSendUpdate = false)
        {
            var result = new AOResult();

            try
            {
                BookModelDTO bookDTO = book;

                await _restService.PutAsync<BookModelDTO>(BASE_URL, bookDTO);

                result.SetSuccess();

                if (shouldSendUpdate)
                {
                    SendUpdateEvent();
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(AddBookAsync)}: exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult<IEnumerable<string>>> GetCategoriesAsync()
        {
            var result = new AOResult<IEnumerable<string>>();

            try
            {
                var categories = await _restService.GetAsync<IEnumerable<string>>(BASE_URL + "/categories");

                if (categories != null)
                {
                    result.SetSuccess(categories);
                }
                else
                {
                    result.SetFailure();
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(GetCategoriesAsync)}: exception", "Something went wrong", ex);
            }

            return result;
        }

        #endregion

        #region -- Private helpers --

        private async Task LoadBooksAsync()
        {
            _loadBooksCompletionSource = new TaskCompletionSource<bool>();

            var result = await _restService.GetAsync<IEnumerable<BookModelDTO>>(BASE_URL);

            if (result != null)
            {
                _books = new List<BookModel>();

                foreach (var book in result)
                {
                    _books.Add(book);
                }

                _loadBooksCompletionSource.SetResult(true);
            }
            else
            {
                _loadBooksCompletionSource.SetResult(false);
            }
        }

        private void RunLoadBooksTask()
        {
            Task.Run(LoadBooksAsync).ConfigureAwait(false);
        }

        private void SendUpdateEvent()
        {
            Task.Run(async () =>
            {
                var tcsResult = await _loadBooksCompletionSource.Task;

                if (tcsResult)
                {
                    _eventAggregator.GetEvent<BooksChangedEvent>().Publish();
                }
            });
        }

        #endregion
    }
}
