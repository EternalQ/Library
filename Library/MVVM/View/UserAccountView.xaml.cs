using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Library.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для UserAccountView.xaml
    /// </summary>
    public partial class UserAccountView : UserControl
    {
        public UserAccountView()
        {
            InitializeComponent();
        }

        private void verifpass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            pass.Password = passtext.Text;
        }

        private void pass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            verifpass.Password = verifpasstext.Text;
        }
    }
}
