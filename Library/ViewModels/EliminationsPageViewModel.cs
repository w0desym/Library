using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Library.Enums;
using Library.Models.Book;
using Library.Services.Book;
using Prism.Navigation;

namespace Library.ViewModels
{
    public class EliminationsPageViewModel : BaseViewModel
    {
        private readonly IBookService _bookService;
        private List<BookBindableModel> _books = new();

        public EliminationsPageViewModel(
            INavigationService navigationService,
            IBookService bookService)
            : base(navigationService)
        {
            _bookService = bookService;
        }

        #region -- Public properties --

        private string _SearchQuery;
        public string SearchQuery
        {
            get => _SearchQuery;
            set => SetProperty(ref _SearchQuery, value);
        }

        private ObservableCollection<BookBindableModel> _Books = new();
        public ObservableCollection<BookBindableModel> Books
        {
            get => _Books;
            set => SetProperty(ref _Books, value);
        }

        private int _SelectedTimePeriod;
        public int SelectedTimePeriod
        {
            get => _SelectedTimePeriod;
            set => SetProperty(ref _SelectedTimePeriod, value);
        }

        private ObservableCollection<int> _TimePeriods;
        public ObservableCollection<int> TimePeriods
        {
            get => _TimePeriods;
            set => SetProperty(ref _TimePeriods, value);
        }

        private EPageState _PageState;
        public EPageState PageState
        {
            get => _PageState;
            set => SetProperty(ref _PageState, value);
        }

        #endregion

        #region -- Overrides --

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadBooksAsync();

            SetPeriods();

            PageState = EPageState.Normal;
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            PageState = EPageState.Loading;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedTimePeriod) || args.PropertyName == nameof(SearchQuery))
            {
                FilterBooks();
            }
        }

        #endregion

        #region -- Private helpers --

        private async Task LoadBooksAsync()
        {
            var result = await _bookService.GetBooksAsync(x => x.EliminationDate is not null);

            if (result.IsSuccess)
            {
                _books.Clear();

                foreach (var book in result.Result)
                {
                    BookBindableModel bookBM = book;
                    _books.Add(bookBM);
                }

                Books = new(_books);
            }
        }

        private void SetPeriods()
        {
            TimePeriods = new() { -7, -30, -90 };

            SelectedTimePeriod = TimePeriods.FirstOrDefault();
        }

        private void FilterBooks()
        {
            Books.Clear();

            var selectedBooks = string.IsNullOrWhiteSpace(SearchQuery)
                ? _books
                : _books.Where(x => x.Name.ToUpper().Contains(SearchQuery?.ToUpper())
                                 || x.Author.ToUpper().Contains(SearchQuery?.ToUpper()));

            Books = new(selectedBooks.Where(x => x.EliminationDate >= DateTime.Now.AddDays(SelectedTimePeriod)));
        }

        #endregion
    }
}
