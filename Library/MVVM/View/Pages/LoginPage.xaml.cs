using Library.Windows;
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
using Library.MVVM.ViewModel;

namespace Library.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            DataContext = LoginViewModel.Instance;
            LoginViewModel.Instance.RootPage = this;
        }

        public void SetPassword(string password)
        {
            txtPassbox.Password = password;
        }

        private void txtPassbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtPassword.Text = txtPassbox.Password;
        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPassbox.Password = txtPassword.Text;
        }
    }
}
