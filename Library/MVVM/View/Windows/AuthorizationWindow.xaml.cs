using Library.Pages;
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
using System.Windows.Shapes;
using Library.MVVM.ViewModel;
using Library.MVVM.Model;

namespace Library.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {

        public AuthorizationWindow()
        {
            InitializeComponent();
            DataContext = AuthWindViewModel.Instance;
            LoginViewModel.Instance.RootWindow = this;

            using (DatabaseContext db = new DatabaseContext())
            {
                Action<bool> succesAction = o =>
                {
                    //Cancel.Execute(o);
                };

                if (db.Users.Count() == 0)
                {
                    RegistrationProvider provider = new RegistrationProvider();
                    provider.SendRequest("Admin", "Admin", succesAction);
                }

                if (db.Tags.Count() == 0)
                {
                    List<Tag> tags = new List<Tag>
                    {
                        new Tag("Classic"),
                        new Tag("Verse"),
                        new Tag("Fiction"),
                        new Tag("Mystery"),
                        new Tag("Thriller"),
                        new Tag("Horror"),
                        new Tag("Historical"),
                        new Tag("Romance"),
                        new Tag("Western"),
                        new Tag("Bildungsroman"),
                        new Tag("Speculative Fiction"),
                        new Tag("Sci-Fi"),
                        new Tag("Fantasy"),
                        new Tag("Dystopian"),
                        new Tag("Magic"),
                        new Tag("Realist Literature"),
                        new Tag("Action"),
                        new Tag("Adventure"),
                        new Tag("Comic")
                    };

                    db.Tags.AddRange(tags);
                    db.SaveChanges();
                    tags = null;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
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
