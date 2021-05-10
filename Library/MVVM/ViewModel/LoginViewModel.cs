using Library.Core;
using Library.MVVM.Model;
using Library.MVVM.View;
using Library.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Library.MVVM.ViewModel
{
    class LoginViewModel : ObservableObject
    {
        public RelayCommand Registration { get; set; }
        public RelayCommand Signin { get; set; }

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

        #region Password
        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region DBError
        private string _dbError;

        public string DatabaseError
        {
            get { return _dbError; }
            set
            {
                _dbError = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region RequestProc
        /// <summary>
        /// Loading?
        /// </summary>
        private bool _isRequestProcessing;

        public bool IsRequestProcessing
        {
            get { return _isRequestProcessing; }
            set
            {
                _isRequestProcessing = value;
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
                output = true;
            }
            return output;
        }

        public Window RootWindow;

        public LoginViewModel()
        {
            Signin = new RelayCommand(o =>
            {
                var provider = new LoginProvider();

                Action succesAction = () =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MainWindow mainwindow = new MainWindow();
                        mainwindow.SetVM(provider.AuthorizedUser);

                        RootWindow.Close();
                        mainwindow.Show();
                    });
                };

                SendRequestTo(provider, succesAction);
            }, o =>
            {
                if (Login == "" || Login == null ||
                Password == "" || Password == null ||
                HasErrors) return false;
                return true;
            });

            Registration = new RelayCommand(o =>
            {
                AuthWindViewModel.Instance.CurrentView = new Uri("../Pages/RegistrationPage.xaml", UriKind.RelativeOrAbsolute);
            });
        }
    }
}
