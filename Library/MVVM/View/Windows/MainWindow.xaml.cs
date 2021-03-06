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
using Library.MVVM.Model;
using Library.MVVM.ViewModel;
using Library.Windows;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //Ошибка
            InitializeComponent();
        }

        public void SetVM(User user)
        {
            DataContext = new MainWindViewModel(user);
        }

        public void LogOut(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow mainwindow = new AuthorizationWindow();

            LocalDataSaver.SetAutologin();
            Close();
            mainwindow.Show();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
