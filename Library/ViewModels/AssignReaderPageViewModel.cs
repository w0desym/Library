using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Library.Enums;
using Library.Events;
using Library.Helpers;
using Library.Models.Book;
using Library.Models.Reader;
using Library.Services.Book;
using Library.Services.Reader;
using Library.Views.Popups;
using Prism.Events;
using Prism.Navigation;

namespace Library.ViewModels
{
    public class AssignReaderPageViewModel : BaseViewModel
    {
        private readonly IReaderService _readerService;
        private readonly IBookService _bookService;
        private List<ReaderBindableModel> _readers = new();
        private BookModel _book;

        public AssignReaderPageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            IReaderService readerService,
            IBookService bookService)
            : base(navigationService)
        {
            _readerService = readerService;
            _bookService = bookService;

            eventAggregator.GetEvent<ReadersChangedEvent>().Subscribe(OnReadersChanged);
        }

        #region -- Public properties --

        private string _SearchQuery;
        public string SearchQuery
        {
            get => _SearchQuery;
            set => SetProperty(ref _SearchQuery, value);
        }

        private ObservableCollection<ReaderBindableModel> _Readers = new();
        public ObservableCollection<ReaderBindableModel> Readers
        {
            get => _Readers;
            set => SetProperty(ref _Readers, value);
        }

        private EPageState _PageState;
        public EPageState PageState
        {
            get => _PageState;
            set => SetProperty(ref _PageState, value);
        }

        private ICommand _AddNewReaderCommand;
        public ICommand AddNewReaderCommand => _AddNewReaderCommand ??= SingleExecutionCommand.FromFunc(OnAddNewReaderAsync);

        #endregion

        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(Constants.Navigation.SELECTED_BOOK, out BookModel book))
            {
                _book = book;
            }
        }

        public override async void OnAppearing()
        {
            await UpdateReaders();

            PageState = EPageState.Normal;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SearchQuery))
            {
                FilterReaders();
            }
        }

        #endregion

        #region -- Private helpers --

        private async void OnReadersChanged()
        {
            await UpdateReaders();
        }

        private async Task UpdateReaders()
        {
            var result = await _readerService.GetReadersAsync();

            if (result.IsSuccess)
            {
                var newReaders = result.Result;

                ICommand readerTappedCommand = SingleExecutionCommand.FromFunc<ReaderBindableModel>(OnReaderTappedCommandAsync);

                _readers.Clear();

                foreach (var reader in newReaders)
                {
                    ReaderBindableModel readerBM = reader;
                    readerBM.TappedCommand = readerTappedCommand;
                    _readers.Add(readerBM);
                }

                Readers = new(_readers);
            }
        }

        private void FilterReaders()
        {
            Readers.Clear();

            var selectedReaders = string.IsNullOrWhiteSpace(SearchQuery)
                ? _readers
                : _readers.Where(x => x.Name.ToUpper().Contains(SearchQuery?.ToUpper())
                                   || x.SecondName.ToUpper().Contains(SearchQuery?.ToUpper())
                                   || x.Surname.ToUpper().Contains(SearchQuery?.ToUpper())
                                   || x.Occupation.ToUpper().Contains(SearchQuery?.ToUpper())
                                   || x.PhoneNumber.ToUpper().Contains(SearchQuery?.ToUpper())
                                   || x.WorkPlace.ToUpper().Contains(SearchQuery?.ToUpper()));

            Readers = new(selectedReaders);
        }

        private Task OnReaderTappedCommandAsync(ReaderBindableModel reader)
        {
            _book.ReaderId = reader.Id;

            _book.FreeDate = DateTime.Now.AddMonths(1);

            _bookService.UpdateBookAsync(_book);

            return NavigationService.GoBackAsync();
        }

        private Task OnAddNewReaderAsync() => NavigationService.NavigateAsync(nameof(NewReaderPopup), null, true, true);

        #endregion
    }
}
