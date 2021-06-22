using Library.Services.Book;
using Library.Services.Reader;
using Library.Services.Rest;
using Library.ViewModels;
using Library.ViewModels.Popups;
using Library.Views;
using Library.Views.Popups;
using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using Prism.Unity;
using Xamarin.Forms;

namespace Library
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            MainPage = new MainTabbedPage();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterPopupNavigationService();

            containerRegistry.Register<NavigationPage>();
            containerRegistry.RegisterForNavigation<CatalogPage>();
            containerRegistry.RegisterForNavigation<ReadersPage>();
            containerRegistry.RegisterForNavigation<EliminationsPage>();

            containerRegistry.RegisterForNavigation<NewReaderPopup, NewReaderPopupViewModel>();
            containerRegistry.RegisterForNavigation<NewBookPopup, NewBookPopupViewModel>();
            containerRegistry.RegisterForNavigation<BookDialogPopup, BookDialogPopupViewModel>();
            containerRegistry.RegisterForNavigation<AssignReaderPage, AssignReaderPageViewModel>();

            containerRegistry.RegisterSingleton<IRestService, RestService>();
            containerRegistry.RegisterSingleton<IReaderService, ReaderService>();
            containerRegistry.RegisterSingleton<IBookService, BookService>();
        }
    }
}
