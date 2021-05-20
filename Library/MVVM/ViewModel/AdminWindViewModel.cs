using Library.Core;
using Library.MVVM.Model;
using Library.MVVM.View.Windows;
using Library.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Library.MVVM.ViewModel
{
    class AdminWindViewModel : ObservableObject
    {
        private static AdminWindViewModel instance;

        public static AdminWindViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AdminWindViewModel();
                }
                return instance;
            }
            set => instance = value;
        }

        BooksAdminViewModel BooksVM;
        UsersViewModel UsersVM;
        AddBooksViewModel AddBookVM;

        public RelayCommand ToUsersCommand { get; set; }
        public RelayCommand ToBooksCommand { get; set; }
        public RelayCommand ToAddBookCommand { get; set; }
        public RelayCommand OffAutoLogin { get; set; }
        public RelayCommand LogOut { get; set; }

        #region CurrentView
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region UserText
        private string _TxtUser;

        public string TxtUser
        {
            get { return _TxtUser; }
            set
            {
                _TxtUser = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region BookText
        private string _TxtBook;

        public string TxtBook
        {
            get { return _TxtBook; }
            set
            {
                _TxtBook = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public AdminWindow RootWindow;

        public AdminWindViewModel()
        {
            BooksVM = new BooksAdminViewModel(null);
            UsersVM = new UsersViewModel();
            AddBookVM = new AddBooksViewModel(null);

            using (DatabaseContext db = new DatabaseContext())
            {
                TxtUser = "Users: " + db.Users.Count();
                TxtBook = "Books: " + db.Books.Count();
            }

            CurrentView = UsersVM;

            ToUsersCommand = new RelayCommand(o =>
            {
                CurrentView = UsersVM;
            });

            ToBooksCommand = new RelayCommand(o =>
            {
                CurrentView = BooksVM;
            });

            ToAddBookCommand = new RelayCommand(o =>
            {
                CurrentView = AddBookVM;
            });

            LogOut = new RelayCommand(o =>
            {
                AuthorizationWindow mainwindow = new AuthorizationWindow();

                LocalDataSaver.SetAutologin();
                RootWindow.Close();
                mainwindow.Show();
            });

            OffAutoLogin = new RelayCommand(o =>
            {
                LocalDataSaver.SetAutologin();
                //MessageBox.Show("hehe");
            }, o =>
            {
                if (LocalDataSaver.GetLoginSettings().IsAutoLoginChecked)
                    return true;
                else
                    return false;
            });
        }
    }
}
