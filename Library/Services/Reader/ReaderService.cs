using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Events;
using Library.Helpers;
using Library.Models.Reader;
using Library.Services.Rest;
using Prism.Events;

namespace Library.Services.Reader
{
    public class ReaderService : IReaderService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRestService _restService;

        private TaskCompletionSource<bool> _loadReadersCompletionSource = new TaskCompletionSource<bool>();

        private string BASE_URL = "https://api-library.azurewebsites.net/api/library/readers";
        private ICollection<ReaderModel> _readers;

        public ReaderService(
            IEventAggregator eventAggregator,
            IRestService restService)
        {
            _eventAggregator = eventAggregator;
            _restService = restService;
            Task.Run(LoadReadersAsync).ConfigureAwait(false);
        }

        #region -- IReaderService implementation --

        public async Task<AOResult<IEnumerable<ReaderModel>>> GetReadersAsync(Func<ReaderModel, bool> func = null)
        {
            var result = new AOResult<IEnumerable<ReaderModel>>();

            try
            {
                RunLoadReadersTask();

                var tcsResult = await _loadReadersCompletionSource.Task;

                if (tcsResult)
                {
                    var readers = func == null
                        ? _readers
                        : _readers.Where(func).OrderBy(x => x.Name).ToList();

                    result.SetSuccess(readers);
                }
                else
                {
                    result.SetFailure();

                    RunLoadReadersTask();
                }
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(GetReadersAsync)}: exception", "Something went wrong", ex);
            }

            return result;
        }

        public async Task<AOResult> AddReaderAsync(ReaderModel reader)
        {
            var result = new AOResult();

            try
            {
                ReaderModelDTO readerDTO = reader;

                await _restService.PostAsync<ReaderModelDTO>(BASE_URL, readerDTO);

                result.SetSuccess();

                SendUpdateEvent();
            }
            catch (Exception ex)
            {
                result.SetError($"{nameof(AddReaderAsync)}: exception", "Something went wrong", ex);
            }

            return result;
        }

        #endregion

        #region -- Private helpers --

        private async Task LoadReadersAsync()
        {
            var result = await _restService.GetAsync<IEnumerable<ReaderModelDTO>>(BASE_URL);

            if (result != null)
            {
                _readers = new List<ReaderModel>();

                foreach (var book in result)
                {
                    _readers.Add(book);
                }

                _loadReadersCompletionSource.SetResult(true);
            }
            else
            {
                _loadReadersCompletionSource.SetResult(false);
            }
        }

        private void RunLoadReadersTask()
        {
            _loadReadersCompletionSource = new TaskCompletionSource<bool>();

            Task.Run(LoadReadersAsync).ConfigureAwait(false);
        }

        private void SendUpdateEvent()
        {
            Task.Run(async () =>
            {
                var tcsResult = await _loadReadersCompletionSource.Task;

                if (tcsResult)
                {
                    _eventAggregator.GetEvent<ReadersChangedEvent>().Publish();
                }
            });
        }

        #endregion
    }
}
