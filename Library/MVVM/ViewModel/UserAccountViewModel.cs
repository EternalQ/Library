using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Library.Core;
using Library.MVVM.Model;

namespace Library.MVVM.ViewModel
{
    class UserAccountViewModel : ObservableObject
    {
        //commands
        public RelayCommand SaveInfoCommand { get; set; }
        public RelayCommand ChangePasswordCommand { get; set; }

        //data
        User baseUser;

        //bindabel properties
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
                Task.Run(() =>
                {
                    using (DatabaseContext db = new DatabaseContext())
                    {
                        var users = db.Users.ToList();
                        if (baseUser != null)
                            users.Remove(db.Users.FirstOrDefault(u => u.Login == baseUser.Login));
                        if (users.Contains(db.Users.FirstOrDefault(u => u.Login == Login)))
                            AddError("This Login already exists");
                    }
                });
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

        #region Password/Verify
        private string _Password;

        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                Dictionary<string, string> loginValidation = new Dictionary<string, string>()
                {
                    {"(?=.*[0-9a-zA-Z])", "Allowed numbers and latin letters only"},
                    {"(?=.*[a-z])", "Pass must contain lower case letter"},
                    {"(?=.*[A-Z])", "Pass must contain higher case letter"},
                    {"(?=.*[0-9])", "Pass must contain numbers"},
                    {"(?=.{6,15})", "Length must be 6-15 symbols"}
                };
                ClearErrors();
                if(_Password != "")
                foreach (var pattern in loginValidation.Keys)
                    if (!Regex.IsMatch(value, pattern))
                    {
                        AddError(loginValidation[pattern]);
                        break;
                    }
                OnPropertyChanged();
            }
        }

        private string _PasswordVerify;

        public string PasswordVerify
        {
            get { return _PasswordVerify; }
            set
            {
                ClearErrors();
                if (Password != value)
                    AddError("Password mismatch");

                _PasswordVerify = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public string ComputeHash(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashConverter = new StringBuilder(128);

            using (var hash = SHA512.Create())
            {
                var hashedBytes = hash.ComputeHash(inputBytes);

                foreach (byte b in hashedBytes)
                {
                    hashConverter.Append(b.ToString("X2"));
                }
            }
            return hashConverter.ToString();
        }

        public UserAccountViewModel() { }

        public UserAccountViewModel(User newuser)
        {
            UserCard card;
            using (DatabaseContext db = new DatabaseContext())
            {
                newuser = baseUser = db.Users.FirstOrDefault(u => u.Login == newuser.Login);
                card = db.UserCards.FirstOrDefault(uc => uc.UserId == newuser.UserId);
            }
            Login = newuser.Login;
            Name = card.FirstName;
            SecondName = card.SecondName;
            Email = card.Email;

            #region SaveUserInfo
            SaveInfoCommand = new RelayCommand(o =>
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    db.Users.Include("Card").FirstOrDefault(u => u.Login == newuser.Login).Login = Login;
                    db.Users.Include("Card").FirstOrDefault(u => u.Login == newuser.Login).Card.Email = Email;
                    db.Users.Include("Card").FirstOrDefault(u => u.Login == newuser.Login).Card.FirstName = Name;
                    db.Users.Include("Card").FirstOrDefault(u => u.Login == newuser.Login).Card.SecondName = SecondName;
                    db.SaveChanges();
                }
            }, o =>
            {
                if (HasErrors)
                    return false;
                return true;
            });
            #endregion

            #region ChangePassword
            ChangePasswordCommand = new RelayCommand(o =>
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    db.Users.FirstOrDefault(u => u.Login == newuser.Login).Password = ComputeHash(Password);
                    db.SaveChanges();
                }
                Password = "";
                PasswordVerify = "";
            }, o =>
            {
                if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(PasswordVerify) || Password != PasswordVerify ||
                PropHasErrors(nameof(Password)) || PropHasErrors(nameof(PasswordVerify)))
                    return false;
                return true;
            });
            #endregion
        }
    }
}
