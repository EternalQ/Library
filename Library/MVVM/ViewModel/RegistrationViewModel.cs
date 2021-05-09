using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Library.Core;
using Library.Pages;
using Library.MVVM.Model;
using System.Windows;
using Library.Windows;
using System.Runtime.CompilerServices;

namespace Library.MVVM.ViewModel
{
    class RegistrationViewModel : ObservableObject
    {
        //public override void OnErrorsChanged([CallerMemberName] string propertyName = null)
        //{
        //    base.OnErrorsChanged(propertyName);
        //    OnPropertyChanged(nameof(RegBtnEnable));
        //}

        AuthorizationWindow mainWindow { get => Application.Current.MainWindow as AuthorizationWindow; }

        public RelayCommand Cancel { get; set; }
        public RelayCommand Registration { get; set; }

        DatabaseContext dbcontext = new DatabaseContext();

        private string _login;

        public string Login
        {
            get { return _login; }
            set
            {
                Dictionary<string, string> loginValidation = new Dictionary<string, string>()
                {
                    {@"(?=.*[0-9a-zA-Z-_\.])", "Allowed numbers, latin letters, _ and dot"},
                    {"(?=^[a-zA-Z])", "Login must start with letter"},
                    {"(?=.{3,15})", "Length must be 3-15 symbols"}
                };
                ClearErrors();
                foreach (var pattern in loginValidation.Keys)
                    if (!Regex.IsMatch(value, pattern))
                    {
                        AddError(loginValidation[pattern]);
                        break;
                    }
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                Dictionary<string, string> loginValidation = new Dictionary<string, string>()
                {
                    {"(?=.*[0-9a-zA-Z])", "Allowed numbers and latin letters only"},
                    {"(?=.*[a-z])", "Pass must contain lower case letter"},
                    {"(?=.*[A-Z])", "Pass must contain higher case letter"},
                    {"(?=.*[0-9])", "Pass must contain numbers"},
                    {"(?=.{6,15})", "Length must be 6-15 symbols"}
                };
                ClearErrors();
                foreach (var pattern in loginValidation.Keys)
                    if (!Regex.IsMatch(value, pattern))
                    {
                        AddError(loginValidation[pattern]);
                        break;
                    }
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _passwordVerify;

        public string PasswordVerify
        {
            get { return _passwordVerify; }
            set
            {
                ClearErrors();
                if (Password != value)
                    AddError("Password mismatch");

                _passwordVerify = value;
                OnPropertyChanged();
            }
        }

        public RegistrationViewModel()
        {
            Cancel = new RelayCommand(o =>
            {
                mainWindow.mainFraim.Navigate(new Uri("/Pages/LoginPage.xaml", UriKind.RelativeOrAbsolute));
                //mainWindow.mainFraim.Content = mainWindow.login;
            });

            Registration = new RelayCommand(o =>
            {
                Task regin = new Task(() =>
                {
                    User user = new User(Login.ToLower(), Password);

                    if (dbcontext.Users.FirstOrDefault(u => u.Login == Login.ToLower()) == null)
                        dbcontext.Users.Add(user);
                    else
                    {

                    }
                });
            },
            o =>
            {
                if (Login == "" || Login == null ||
                Password == "" || Password == null ||
                PasswordVerify == "" || PasswordVerify == null ||
                HasErrors) return false;
                return true;
            });
        }
    }
}
