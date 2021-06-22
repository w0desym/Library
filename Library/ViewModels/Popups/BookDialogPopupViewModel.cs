using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Library.Helpers;
using Library.Models.Book;
using Library.Services.Book;
using Library.Views;
using Prism.Navigation;

namespace Library.ViewModels.Popups
{
    public class BookDialogPopupViewModel : BaseViewModel
    {
        private readonly IBookService _bookService;

        private INavigationParameters _parameters;
        private BookModel _book;
        private bool _isBookAssigned;

        public BookDialogPopupViewModel(
            INavigationService navigationService,
            IBookService bookService)
            : base(navigationService)
        {
            _bookService = bookService;
        }

        #region -- Public properties --

        private string _FirstActionText;
        public string FirstActionText
        {
            get => _FirstActionText;
            set => SetProperty(ref _FirstActionText, value);
        }

        private string _SecondActionText = "Eliminate";
        public string SecondActionText
        {
            get => _SecondActionText;
            set => SetProperty(ref _SecondActionText, value);
        }

        private bool _IsSecondActionEnabled = true;
        public bool IsSecondActionEnabled
        {
            get => _IsSecondActionEnabled;
            set => SetProperty(ref _IsSecondActionEnabled, value);
        }

        private ICommand _FirstActionCommand;
        public ICommand FirstActionCommand => _FirstActionCommand ??= SingleExecutionCommand.FromFunc(OnFirstActionCommandAsync);

        private ICommand _SecondActionCommand;
        public ICommand SecondActionCommand => _SecondActionCommand ??= SingleExecutionCommand.FromFunc(OnSecondActionCommandAsync);

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _parameters = parameters;

            if (parameters.TryGetValue(Constants.Navigation.SELECTED_BOOK, out BookModel book))
            {
                _book = book;

                _isBookAssigned = book.ReaderId is not null;

                if (_isBookAssigned)
                {
                    FirstActionText = "Return";
                    IsSecondActionEnabled = false;
                }
                else
                {
                    FirstActionText = "To Hands";
                }
            }
        }

        #endregion

        #region -- Private helpers --

        private async Task OnFirstActionCommandAsync()
        {
            if (_isBookAssigned)
            {
                _book.ReaderId = null;

                _book.FreeDate = null;

                await _bookService.UpdateBookAsync(_book, true);

                await NavigationService.GoBackAsync();
            }
            else
            {
                await NavigationService.NavigateAsync(nameof(AssignReaderPage), _parameters);
            }
        }

        private async Task OnSecondActionCommandAsync()
        {
            _book.EliminationDate = DateTime.Now;

            await _bookService.UpdateBookAsync(_book, true);

            await NavigationService.GoBackAsync();
        }

        #endregion
    }
}
