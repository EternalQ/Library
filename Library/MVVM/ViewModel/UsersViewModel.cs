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
        public RelayCommand SaveUserInfo { get; set; }

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
                    {@"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$", "Wrong E-Mail"}
                };
                ClearErrors();
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
            });
        }
    }
}
