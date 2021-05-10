using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Library.MVVM.Model
{
    public class User
    {
        //[Identity]
        public int UserId { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        [Required]
        public UserCard Card { get; set; }
        public List<Book> FavBooks { get; set; }

        public User() { }

        public User(string login, string password)
        {
            Login = login;
            Password = password;
            Card = new UserCard();
        }
    }
}
