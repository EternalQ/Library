using Library.Core;
using Library.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.MVVM.ViewModel
{
    class MainWindViewModel : ObservableObject
    {


        #region CurrentView
        private object _CurrentView;

        public object Currentview
        {
            get { return _CurrentView; }
            set
            {
                _CurrentView = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public MainWindViewModel() { }

        public MainWindViewModel(User user)
        {

        }
    }
}
