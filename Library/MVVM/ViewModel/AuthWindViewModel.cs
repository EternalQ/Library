using Library.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.MVVM.ViewModel
{
    class AuthWindViewModel : ObservableObject
    {
        #region Singleton
        private static AuthWindViewModel instance;

        public static AuthWindViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AuthWindViewModel();
                }
                return instance;
            }
            set => instance = value;
        }
        #endregion

        #region CurrentView
        private Uri _currentView;

        public Uri CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public AuthWindViewModel()
        {
            CurrentView = new Uri("../Pages/LoginPage.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}
