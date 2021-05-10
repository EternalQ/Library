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
            DataContext = new LoginViewModel();
        }

        //AuthorizationWindow aWindow { get => Application.Current.MainWindow as AuthorizationWindow; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //aWindow.mainFraim.Navigate(new Uri("MVVM/View/Pages/RegisrationPage.xaml", UriKind.RelativeOrAbsolute));
            //aWindow.SwapPage();
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    MainWindow main = new MainWindow();
        //    main.Show();
        //    aWindow.Close();
        //}

        private void txtPassbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtPassword.Text = txtPassbox.Password;
        }
    }
}
