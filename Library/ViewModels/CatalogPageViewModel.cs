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
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Library.ViewModels
{
    public class CatalogPageViewModel : BaseViewModel
    {
        private const string ALL_GENRES = "All Genres";

        private readonly IBookService _bookService;
        private readonly IReaderService _readerService;
        private HashSet<Tuple<BookBindableModel, ReaderBindableModel>> _booksAndReaders = new();

        public CatalogPageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            IBookService bookService,
            IReaderService readerService)
            : base(navigationService)
        {
            _bookService = bookService;
            _readerService = readerService;

            eventAggregator.GetEvent<BooksChangedEvent>().Subscribe(OnBooksChanged);
        }

        #region -- Public properties --

        private ObservableCollection<string> _Categories;
        public ObservableCollection<string> Categories
        {
            get => _Categories;
            set => SetProperty(ref _Categories, value);
        }

        private ObservableCollection<Tuple<BookBindableModel, ReaderBindableModel>> _BooksAndReaders = new();
        public ObservableCollection<Tuple<BookBindableModel, ReaderBindableModel>> BooksAndReaders
        {
            get => _BooksAndReaders;
            set => SetProperty(ref _BooksAndReaders, value);
        }

        private string _SearchQuery;
        public string SearchQuery
        {
            get => _SearchQuery;
            set => SetProperty(ref _SearchQuery, value);
        }

        private string _SelectedCategory;
        public string SelectedCategory
        {
            get => _SelectedCategory;
            set => SetProperty(ref _SelectedCategory, value);
        }

        private EPageState _PageState;
        public EPageState PageState
        {
            get => _PageState;
            set => SetProperty(ref _PageState, value);
        }

        private ICommand _AddNewBookCommand;
        public ICommand AddNewBookCommand => _AddNewBookCommand ??= SingleExecutionCommand.FromFunc(OnAddNewBookAsync);

        #endregion

        #region -- Overrides --

        public override async void OnAppearing()
        {
            await LoadBooksAndReadersAsync();

            await LoadCategoriesAsync();

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

            if (args.PropertyName == nameof(SelectedCategory) || args.PropertyName == nameof(SearchQuery))
            {
                FilterBooksAndReaders();
            }
        }

        #endregion

        #region -- Private helpers --

        private async void OnBooksChanged()
        {
            await LoadBooksAndReadersAsync();
        }

        private async Task LoadBooksAndReadersAsync()
        {
            var results = await Task.WhenAll(LoadBooksAsync(), LoadReadersAsync());

            var books = results[0] as IEnumerable<BookBindableModel>;

            var readers = results[1] as IEnumerable<ReaderBindableModel>;

            _booksAndReaders.Clear();

            foreach (var book in books)
            {
                var reader = readers.FirstOrDefault(x => x.Id == book.ReaderId);

                _booksAndReaders.Add(new(book, reader));
            }

            BooksAndReaders = new(_booksAndReaders);
        }

        private async Task<IEnumerable<object>> LoadBooksAsync()
        {
            var books = new List<BookBindableModel>();

            var result = await _bookService.GetBooksAsync(x => x.EliminationDate == null);

            if (result.IsSuccess)
            {
                var newBooks = result.Result;

                ICommand bookLongPressedCommand = SingleExecutionCommand.FromFunc<BookBindableModel>(OnBookSelectedCommandAsync);

                foreach (var book in newBooks)
                {
                    BookBindableModel bookBM = book;
                    bookBM.LongPressedCommand = bookLongPressedCommand;
                    books.Add(bookBM);
                }
            }

            return books;
        }

        private async Task<IEnumerable<object>> LoadReadersAsync()
        {
            var readers = new List<ReaderBindableModel>();

            var result = await _readerService.GetReadersAsync();

            if (result.IsSuccess)
            {
                foreach (var reader in result.Result)
                {
                    readers.Add(reader);
                }
            }

            return readers;
        }

        private async Task LoadCategoriesAsync()
        {
            var result = await _bookService.GetCategoriesAsync();

            if (result.IsSuccess)
            {
                Categories = new(result.Result);

                Categories.Insert(0, ALL_GENRES);

                SelectedCategory = Categories.FirstOrDefault();
            }
        }

        private void FilterBooksAndReaders()
        {
            BooksAndReaders.Clear();

            var selectedBooksAndReaders = string.IsNullOrWhiteSpace(SearchQuery)
                ? _booksAndReaders
                : _booksAndReaders.Where(x => x.Item1.Name.ToUpper().Contains(SearchQuery?.ToUpper())
                                           || x.Item1.Author.ToUpper().Contains(SearchQuery?.ToUpper()));

            if (SelectedCategory == ALL_GENRES)
            {
                BooksAndReaders = new(selectedBooksAndReaders);
            }
            else
            {
                BooksAndReaders = new(selectedBooksAndReaders.Where(x => x.Item1.Category == SelectedCategory));
            }
        }

        private Task OnAddNewBookAsync() => NavigationService.NavigateAsync(nameof(NewBookPopup), null, true, true);

        private Task OnBookSelectedCommandAsync(BookBindableModel book)
        {
            var parameters = new NavigationParameters
            {
                { Constants.Navigation.SELECTED_BOOK, (BookModel)book }
            };

            return NavigationService.NavigateAsync(nameof(BookDialogPopup), parameters);
        }

        #endregion
    }
}

