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
        public RelayCommand Cancel { get; set; }
        public RelayCommand Registration { get; set; }
        public RelayCommand ToLogin { get; set; }

        #region Login
        private string _Login;

        public string Login
        {
            get { return _Login; }
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
                _Login = value;
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
                _Password = value;
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

        #region DBError
        /// <summary>
        /// Database connection errors
        /// </summary>
        private string _DBError;

        public string DatabaseError
        {
            get { return _DBError; }
            set
            {
                _DBError = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region RequestProc
        /// <summary>
        /// Loading?
        /// </summary>
        private bool _IsRequestProcessing;

        public bool IsRequestProcessing
        {
            get { return _IsRequestProcessing; }
            set
            {
                _IsRequestProcessing = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private bool SendRequestTo(BaseProvider provider, Action succesCallback)
        {
            DatabaseError = string.Empty;
            bool output = false;

            if (IsRequestProcessing == true)
                return false;

            if (HasErrors == false)
            {
                Action<bool> callback = (succes) =>
                {
                    IsRequestProcessing = false;

                    if (succes == false)
                    {
                        DatabaseError = provider.Error;
                    }
                    else
                    {
                        succesCallback?.Invoke();
                    }
                };
                IsRequestProcessing = true;
                provider.SendRequest(Login.Trim(), Password.Trim(), callback);
                //provider.SendRequest("Admin", "Admin", callback);
                output = true;
            }
            return output;
        }
        
        public RegistrationViewModel()
        {
            Registration = new RelayCommand(o =>
            {
                Action succesAction = () =>
                {
                    Cancel.Execute(o);
                };

                SendRequestTo(new RegistrationProvider(), succesAction);
            },
            o =>
            {
                if (Login == "" || Login == null ||
                Password == "" || Password == null ||
                PasswordVerify == "" || PasswordVerify == null ||
                HasErrors) return false;
                return true;
            });

            Cancel = new RelayCommand(o =>
            {
                AuthWindViewModel.Instance.CurrentView = new Uri("../Pages/LoginPage.xaml", UriKind.RelativeOrAbsolute);
            });
        }
    }
}
