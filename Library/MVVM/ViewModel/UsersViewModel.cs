using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Library.Core;
using Library.MVVM.Model;

namespace Library.MVVM.ViewModel
{
    class UsersViewModel : ObservableObject
    {
        public RelayCommand DeleteSelectedUser { get; set; }
        public RelayCommand OpenSelectedBook { get; set; }
        public RelayCommand SaveUserInfo { get; set; }
        public RelayCommand Test { get; set; }
        //public RelayCommand ChangeUserInfo { get; set; }

        #region UsersList
        private List<User> _userList;

        public List<User> UserList
        {
            get { return _userList; }
            set
            {
                _userList = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region SelectedItem
        private User _selectedUser;

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
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
                selected = db.Users.FirstOrDefault(u => u.UserId == SelectedUser.UserId);
                card = db.UserCards.FirstOrDefault(u => u.UserId == SelectedUser.UserId);
            }

            Login = selected.Login;
            Name = card.FirstName;
            SecondName = card.SecondName;
            Email = card.Email;
            IsUserSelected = true;
            UserBookList = selected.FavBooks;
        }
        #endregion

        #region Selection
        private bool _isSelected;

        public bool IsUserSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Login
        private string _login;

        public string Login
        {
            get { return _login; }
            set
            {
                ClearErrors();
                if (value == "" || value == null)
                    AddError("Login can't be empty");
                _login = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Name
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region SecondName
        private string _secName;

        public string SecondName
        {
            get { return _secName; }
            set
            {
                _secName = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Email
        private string _email;

        public string Email
        {
            get { return _email; }
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
                _email = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region BookList
        private List<Book> _bookList;

        public List<Book> UserBookList
        {
            get { return _bookList; }
            set
            {
                _bookList = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region SelectedBook
        private Book _book;

        public Book SelectedBook
        {
            get { return _book; }
            set
            {
                _book = value;
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
                if (SelectedUser == null)
                    return false;
                else
                    return true;
            });

            OpenSelectedBook = new RelayCommand(o =>
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    var selected = db.Books.FirstOrDefault(u => u.BookId == SelectedBook.BookId);
                    AdminWindViewModel.Instance.ToBooksCommand.Execute(null);
                }
            }, o =>
            {
                if (SelectedBook == null)
                    return false;
                else
                    return true;
            });

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

            Test = new RelayCommand(o =>
            {
                MessageBox.Show("working");
            });
        }
    }
}
