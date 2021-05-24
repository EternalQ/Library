using Library.MVVM.Model;
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

namespace Library.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для BooksView.xaml
    /// </summary>
    public partial class BooksView : UserControl
    {
        public BooksView()
        {
            InitializeComponent();

        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Book book = BookList.SelectedItem as Book;
            using (DatabaseContext db = new DatabaseContext())
            {
                User user = BooksViewModel.Instance.Account;
                db.Users.Include("Books").FirstOrDefault(u=>u.UserId == user.UserId).Books.Add(db.Books.FirstOrDefault(b => b.Name == book.Name));
                db.SaveChanges();
            }
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Book book = BookList.SelectedItem as Book;
            using (DatabaseContext db = new DatabaseContext())
            {
                User user = BooksViewModel.Instance.Account;
                db.Users.Include("Books").FirstOrDefault(u => u.UserId == user.UserId).Books.Remove(db.Books.FirstOrDefault(b => b.Name == book.Name));
                db.SaveChanges();
            }
        }
    }
}
