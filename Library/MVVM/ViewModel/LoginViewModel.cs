using Library.Core;
using Library.MVVM.Model;
using Library.MVVM.View.Windows;
using Library.Pages;
using System;
using System.Windows;

namespace Library.MVVM.ViewModel
{
    class LoginViewModel : ObservableObject
    {
        #region Singleton
        private static LoginViewModel instance;

        public static LoginViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoginViewModel();
                }
                return instance;
            }
            set => instance = value;
        } 
        #endregion

        public RelayCommand Registration { get; set; }
        public RelayCommand Signin { get; set; }

        #region Login
        private string _Login;

        public string Login
        {
            get { return _Login; }
            set
            {
                _Login = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Password
        private string _Password;

        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region DBError
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

        #region CheckBox1
        private bool _IsRemember;

        public bool IsRememberChecked
        {
            get { return _IsRemember; }
            set
            {
                _IsRemember = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region CheckBox2
        private bool _IsAutologin;

        public bool IsAutologinChecked
        {
            get { return _IsAutologin; }
            set
            {
                _IsAutologin = value;
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
                        IsAutologinChecked = false;
                        LocalDataSaver.SaveLoginSettings(Login, Password, IsRememberChecked, IsAutologinChecked);
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

        private void OnSignin(object o)
        {
            if (IsRememberChecked)
                LocalDataSaver.SaveLoginSettings(Login, Password, IsRememberChecked, IsAutologinChecked);
            else
                LocalDataSaver.SaveLoginSettings("", "", IsRememberChecked, IsAutologinChecked);

            var provider = new LoginProvider();

            Action succesAction = () =>
            {
                switch (Login.Trim().ToLower())
                {
                    case "admin":
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                AdminWindow mainwindow = new AdminWindow();

                                RootWindow.Close();
                                mainwindow.Show();
                            });
                            break;
                        }
                    default:
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                MainWindow mainwindow = new MainWindow();
                                mainwindow.SetVM(provider.AuthorizedUser);

                                RootWindow.Close();
                                mainwindow.Show();
                            });
                            break;
                        }
                }
            };

            SendRequestTo(provider, succesAction);
        }

        public Window RootWindow;
        public LoginPage RootPage;

        public LoginViewModel()
        {
            LocalDataSaver.GetLoginSettings(out _Login, out _Password, out _IsRemember, out _IsAutologin);

            if (IsAutologinChecked && IsRememberChecked)
            {
                OnSignin(null);
            }

            Signin = new RelayCommand(OnSignin, o =>
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
