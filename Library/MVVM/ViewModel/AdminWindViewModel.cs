using Library.Core;
using Library.MVVM.Model;
using Library.MVVM.View.Windows;
using Library.Windows;
using System.Linq;

namespace Library.MVVM.ViewModel
{
    class AdminWindViewModel : ObservableObject
    {
        #region Singleton
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
        #endregion

        //Views of admin panel
        BooksAdminViewModel BooksVM;
        UsersViewModel UsersVM;
        AddBooksViewModel AddBookVM;

        //Commands
        public RelayCommand ToUsersCommand { get; set; }
        public RelayCommand ToBooksCommand { get; set; }
        public RelayCommand ToAddBookCommand { get; set; }
        public RelayCommand OffAutoLoginCommand { get; set; }
        public RelayCommand LogOutCommand { get; set; }

        //bindable properties
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

            LogOutCommand = new RelayCommand(o =>
            {
                AuthorizationWindow mainwindow = new AuthorizationWindow();

                LocalDataSaver.SetAutologin();
                RootWindow.Close();
                mainwindow.Show();
            });

            OffAutoLoginCommand = new RelayCommand(o =>
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
