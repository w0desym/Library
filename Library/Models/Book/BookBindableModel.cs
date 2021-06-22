using System;
using System.Windows.Input;
using Prism.Mvvm;

namespace Library.Models.Book
{
    public class BookBindableModel : BindableBase
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

        private string _Author;
        public string Author
        {
            get => _Author;
            set => SetProperty(ref _Author, value);
        }

        private string _Category;
        public string Category
        {
            get => _Category;
            set => SetProperty(ref _Category, value);
        }

        private int? _ReaderId;
        public int? ReaderId
        {
            get => _ReaderId;
            set => SetProperty(ref _ReaderId, value);
        }

        private DateTime? _FreeDate = null;
        public DateTime? FreeDate
        {
            get => _FreeDate;
            set => SetProperty(ref _FreeDate, value);
        }

        private DateTime? _EliminationDate;
        public DateTime? EliminationDate
        {
            get => _EliminationDate;
            set => SetProperty(ref _EliminationDate, value);
        }

        private ICommand _LongPressedCommand;
        public ICommand LongPressedCommand
        {
            get => _LongPressedCommand;
            set => SetProperty(ref _LongPressedCommand, value);
        }

        #endregion
    }
}
