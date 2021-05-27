using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Library.Core;
using Library.MVVM.Model;

namespace Library.MVVM.ViewModel
{
    class UsersViewModel : ObservableObject
    {
        public RelayCommand DeleteSelectedUser { get; set; }
        public RelayCommand OpenSelectedBook { get; set; }
        public RelayCommand SaveUserInfo { get; set; }
        //public RelayCommand Test { get; set; }
        //public RelayCommand ChangeUserInfo { get; set; }

        #region UsersList
        private List<User> _UserList;

        public List<User> UserList
        {
            get { return _UserList; }
            set
            {
                _UserList = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region SelectedItem
        private User _SelectedUser;

        public User SelectedUser
        {
            get { return _SelectedUser; }
            set
            {
                _SelectedUser = value;
                OnPropertyChanged();
                ChangeUserInfo();
            }
        }

        private void ChangeUserInfo()
        {
            User selected;
            UserCard card;
            using (DatabaseContext db = new DatabaseContext())
            {
                selected = db.Users.Include("Books").FirstOrDefault(u => u.UserId == SelectedUser.UserId);
                card = db.UserCards.FirstOrDefault(u => u.UserId == SelectedUser.UserId);
            }

            Login = selected.Login;
            Name = card.FirstName;
            SecondName = card.SecondName;
            Email = card.Email;
            IsUserSelected = true;
            UserBookList = selected.Books;
        }
        #endregion

        #region Selection
        private bool _IsSelected;

        public bool IsUserSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Login
        private string _Login;

        public string Login
        {
            get { return _Login; }
            set
            {
                ClearErrors();
                if (value == "" || value == null)
                    AddError("Login can't be empty");
                if (!Regex.IsMatch(value, @"^[a-zA-Z-_\.\s]*$"))
                    AddError("Latins letters only");
                _Login = value;
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
                ClearErrors();
                if (!Regex.IsMatch(value, @"^[a-zA-Z-\-\s]*$"))
                    AddError("Latins letters only");
                OnPropertyChanged();
            }
        }
        #endregion

        #region SecondName
        private string _SecName;

        public string SecondName
        {
            get { return _SecName; }
            set
            {
                _SecName = value;
                ClearErrors();
                if (!Regex.IsMatch(value, @"^[a-zA-Z-\-\s]*$"))
                    AddError("Latins letters only");
                OnPropertyChanged();
            }
        }
        #endregion

        #region Email
        private string _Email;

        public string Email
        {
            get { return _Email; }
            set
            {
                //validation
                Dictionary<string, string> emailValidation = new Dictionary<string, string>()
                {
                    //{@"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$", "Wrong E-Mail"},
                    {@"^[a-zA-Z].[a-zA-Z0-9]*@[a-zA-Z].[a-zA-Z0-9]*\.[a-zA-Z0-9]{2,6}$", "Wrong E-Mail"}
                };
                ClearErrors();
                if (value != null)
                    foreach (var pattern in emailValidation.Keys)
                        if (!Regex.IsMatch(value, pattern))
                        {
                            AddError(emailValidation[pattern]);
                            break;
                        }
                _Email = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region UserSearch
        private string _UserSearch;

        public string UserSearch
        {
            get { return _UserSearch; }
            set
            {
                _UserSearch = value;
                OnPropertyChanged();
                using (DatabaseContext db = new DatabaseContext())
                {
                    if (UserSearch != "")
                        UserList = db.Users.Where(u => u.Login.Trim().Contains(UserSearch.Trim())).ToList();
                    else
                        UserList = db.Users.ToList();
                }
            }
        }
        #endregion

        #region BookSearch
        private string _BookSearch;

        public string BookSearch
        {
            get { return _BookSearch; }
            set
            {
                _BookSearch = value;
                OnPropertyChanged();
                using (DatabaseContext db = new DatabaseContext())
                {
                    if (BookSearch != "")
                        UserBookList = db.Books.Where(b => b.Name.Trim().Contains(BookSearch.Trim())).ToList();
                    else
                        UserBookList = db.Books.ToList();
                }
            }
        }
        #endregion

        #region BookList
        private List<Book> _BookList;

        public List<Book> UserBookList
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
        private Book _Book;

        public Book SelectedBook
        {
            get { return _Book; }
            set
            {
                _Book = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public UsersViewModel()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                UserList = db.Users.ToList();
            }

            #region DeleteUser
            DeleteSelectedUser = new RelayCommand(o =>
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    var selected = db.Users.FirstOrDefault(u => u.UserId == SelectedUser.UserId);
                    var card = db.UserCards.FirstOrDefault(u => u.UserId == SelectedUser.UserId);
                    //MessageBox.Show(card.UserId.ToString());
                    db.UserCards.Remove(selected.Card);
                    db.Users.Remove(selected);
                    db.SaveChanges();
                    UserList = db.Users.ToList();
                }
            }, o =>
            {
                if (SelectedUser == null || SelectedUser.Login.ToLower() == "admin")
                    return false;
                else
                    return true;
            });
            #endregion

            #region OpenBook
            OpenSelectedBook = new RelayCommand(o =>
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    var selected = db.Books.FirstOrDefault(u => u.BookId == SelectedBook.BookId);
                    BooksAdminViewModel.Instance.BookEdit.Execute(selected);
                    //AdminWindViewModel.Instance.ToBooksCommand.Execute(null);
                }
            }, o =>
            {
                if (SelectedBook == null)
                    return false;
                else
                    return true;
            });
            #endregion

            #region SaveUserInfo
            SaveUserInfo = new RelayCommand(o =>
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    db.Users.FirstOrDefault(u => u.UserId == SelectedUser.UserId).Login = Login;
                    db.UserCards.FirstOrDefault(u => u.UserId == SelectedUser.UserId).Email = Email;
                    db.UserCards.FirstOrDefault(u => u.UserId == SelectedUser.UserId).FirstName = Name;
                    db.UserCards.FirstOrDefault(u => u.UserId == SelectedUser.UserId).SecondName = SecondName;
                    db.SaveChanges();
                }
            }, o =>
            {
                if (Login == "" || Login == null ||
                HasErrors || SelectedUser == null)
                    return false;
                else
                    return true;
            });
            #endregion
        }
    }
}
