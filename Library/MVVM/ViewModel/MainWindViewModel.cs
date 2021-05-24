using Library.Core;
using Library.MVVM.Model;
using Library.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Library.MVVM.ViewModel
{
    class MainWindViewModel : ObservableObject
    {
        #region Singleton
        private static MainWindViewModel instance;

        public static MainWindViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainWindViewModel();
                }
                return instance;
            }
            set => instance = value;
        }
        #endregion

        //Views of main window
        BooksViewModel BooksVM;
        public AddBookUserViewModel AddBookVM;
        UserAccountViewModel AccountVM;

        //Commands
        public RelayCommand BooksViewCommand { get; set; }
        public RelayCommand AddBookViewCommand { get; set; }
        public RelayCommand AccountViewCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        //Bindable properties
        #region CurrentView
        private object _CurrentView;

        public object CurrentView
        {
            get { return _CurrentView; }
            set
            {
                _CurrentView = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Dialog
        private bool _Dialog;

        public bool Dialog
        {
            get { return _Dialog; }
            set { _Dialog = value; OnPropertyChanged(); }
        }
        #endregion

        public MainWindViewModel() { }

        public MainWindViewModel(User user)
        {
            BooksVM = new BooksViewModel(user);
            AddBookVM = new AddBookUserViewModel();
            AccountVM = new UserAccountViewModel(user);

            CurrentView = BooksVM;

            #region BooksView
            BooksViewCommand = new RelayCommand(o =>
            {
                CurrentView = BooksVM;
            });
            #endregion

            #region AddBookView
            AddBookViewCommand = new RelayCommand(o =>
            {
                CurrentView = AddBookVM;
            });
            #endregion

            #region AccountView
            AccountViewCommand = new RelayCommand(o =>
            {
                CurrentView = AccountVM;
            });
            #endregion

            Instance = this;
        }
    }
}
