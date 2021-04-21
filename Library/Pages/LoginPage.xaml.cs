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
        }

        AuthorizationWindow aWindow { get => Application.Current.MainWindow as AuthorizationWindow; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            aWindow.mainFraim.Navigate(new Uri("/Pages/RegisrationPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            aWindow.Close();
        }
    }
}
