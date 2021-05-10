using Library.Windows;
using Library.MVVM.ViewModel;
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

namespace Library.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisrationPage.xaml
    /// </summary>
    public partial class RegisrationPage : Page
    {
        public RegisrationPage()
        {
            InitializeComponent();
            DataContext = new RegistrationViewModel();
        }

        private void txtPassbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtPassword.Text = txtPassbox.Password;
        }

        private void txtPassVerifbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtPasswordVerify.Text = txtPassVerifbox.Password;
        }
    }
}
