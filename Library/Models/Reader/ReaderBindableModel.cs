using System;
using Library.Helpers;
using System.Windows.Input;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace Library.Models.Reader
{
    public class ReaderBindableModel : BindableBase
    {
        #region -- Public properties --

        private int _Id;
        public int Id
        {
            get => _Id;
            set => SetProperty(ref _Id, value);
        }

        private string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        private string _SecondName;
        public string SecondName
        {
            get => _SecondName;
            set => SetProperty(ref _SecondName, value);
        }

        private string _Surname;
        public string Surname
        {
            get => _Surname;
            set => SetProperty(ref _Surname, value);
        }

        private DateTime? _BirthDate = DateTime.Now;
        public DateTime? BirthDate
        {
            get => _BirthDate;
            set => SetProperty(ref _BirthDate, value);
        }

        private string _Occupation;
        public string Occupation
        {
            get => _Occupation;
            set => SetProperty(ref _Occupation, value);
        }

        private string _WorkPlace;
        public string WorkPlace
        {
            get => _WorkPlace;
            set => SetProperty(ref _WorkPlace, value);
        }

        private string _PhoneNumber;
        public string PhoneNumber
        {
            get => _PhoneNumber;
            set => SetProperty(ref _PhoneNumber, value);
        }

        private string _Passport;
        public string Passport
        {
            get => _Passport;
            set => SetProperty(ref _Passport, value);
        }

        private ICommand _TappedCommand;
        public ICommand TappedCommand
        {
            get => _TappedCommand;
            set => SetProperty(ref _TappedCommand, value);
        }

        #endregion
    }
}
