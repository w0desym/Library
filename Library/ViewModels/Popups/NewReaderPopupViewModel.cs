using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Library.Helpers;
using Library.Models.Reader;
using Library.Services.Reader;
using Prism.Navigation;

namespace Library.ViewModels.Popups
{
    public class NewReaderPopupViewModel : BaseViewModel
    {
        private readonly IReaderService _readerService;

        public NewReaderPopupViewModel(
            INavigationService navigationService,
            IReaderService readerService)
            : base(navigationService)
        {
            _readerService = readerService;
        }

        #region -- Public properties --

        private ReaderBindableModel _Reader = new();
        public ReaderBindableModel Reader
        {
            get => _Reader;
            set => SetProperty(ref _Reader, value);
        }

        private ICommand _AddCommand;
        public ICommand AddCommand => _AddCommand ??= SingleExecutionCommand.FromFunc(OnAddCommandAsync, () => false);

        #endregion

        #region -- Overrides --



        #endregion

        #region -- Private helpers --

        private async Task OnAddCommandAsync()
        {
            await _readerService.AddReaderAsync(Reader);
            await NavigationService.GoBackAsync();
        }

        #endregion
    }
}
