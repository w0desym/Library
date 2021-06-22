using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Library.Helpers;
using Library.Models.Book;
using Library.Services.Book;
using Prism.Navigation;

namespace Library.ViewModels.Popups
{
    public class NewBookPopupViewModel : BaseViewModel
    {
        private readonly IBookService _bookService;

        public NewBookPopupViewModel(
            INavigationService navigationService,
            IBookService bookService)
            : base(navigationService)
        {
            _bookService = bookService;
        }

        #region -- Public properties --

        private BookBindableModel _Book = new();
        public BookBindableModel Book
        {
            get => _Book;
            set => SetProperty(ref _Book, value);
        }

        private ObservableCollection<string> _Categories = new();
        public ObservableCollection<string> Categories
        {
            get => _Categories;
            set => SetProperty(ref _Categories, value);
        }

        private ICommand _AddCommand;
        public ICommand AddCommand => _AddCommand ??= SingleExecutionCommand.FromFunc(OnAddCommandAsync, () => false);

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            InitCategories();
        }

        #endregion

        #region -- Private helpers --

        private async Task OnAddCommandAsync()
        {
            await _bookService.AddBookAsync(Book);
            await NavigationService.GoBackAsync();
        }

        private async void InitCategories()
        {
            var result = await _bookService.GetCategoriesAsync();

            if (result.IsSuccess)
            {
                var categories = result.Result;

                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                Book.Category = Categories.FirstOrDefault();
            }
        }

        #endregion
    }
}
