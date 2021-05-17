using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Core;

namespace Library.MVVM.ViewModel
{
    class BooksAdminViewModel : ObservableObject
    {
        #region BookList
        private string _BookList;

        public string BookList
        {
            get { return _BookList; }
            set
            {
                _BookList = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region SelectedBook
        private string _SelectedBook;

        public string SelectedBook
        {
            get { return _SelectedBook; }
            set
            {
                _SelectedBook = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Name
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region AuthorName
        private string _AuthorName;

        public string AuthorName
        {
            get { return _AuthorName; }
            set
            {
                _AuthorName = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Description
        private string _Description;

        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region IsFB2
        private bool _IsFB2;

        public bool IsFB2
        {
            get { return _IsFB2; }
            set
            {
                _IsFB2 = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region IsEPUB
        private bool _IsEPUB;

        public bool IsEPUB
        {
            get { return _IsEPUB; }
            set
            {
                _IsEPUB = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public BooksAdminViewModel()
        {

        }
    }
}
