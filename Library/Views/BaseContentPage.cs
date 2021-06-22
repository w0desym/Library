using Library.Interfaces;
using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using System;
using Page = Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page;

namespace Library.Views
{
    public class BaseContentPage : ContentPage
    {
        public BaseContentPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            ViewModelLocator.SetAutowireViewModel(this, true);

            Page.SetUseSafeArea(On<iOS>(), true);
        }

        #region -- Overrides --

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is IAppearingAware vm)
            {
                vm.OnAppearing();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (BindingContext is IAppearingAware vm)
            {
                vm.OnDisappearing();
            }
        }

        #endregion
    }
}