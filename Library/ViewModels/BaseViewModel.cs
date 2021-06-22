using Library.Interfaces;
using Prism.Mvvm;
using Prism.Navigation;

namespace Library.ViewModels
{
    public abstract class BaseViewModel : BindableBase, IInitialize, INavigationAware, IDestructible, IAppearingAware
    {
        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        #region -- Public properties --

        protected INavigationService NavigationService { get; private set; }

        #endregion

        #region -- IInitialize implementation --

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        #endregion

        #region -- INavigationAware implementation --

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        #endregion

        #region -- IDestructible implementation --

        public virtual void Destroy()
        {
        }

        #endregion

        #region -- IAppearingAware implementation --

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }

        #endregion
    }
}
