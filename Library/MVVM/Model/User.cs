using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.MVVM.Model
{
    public class User
    {
        //[Identity]
        public int UserId { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        
        /// <summary>
        /// User additive info
        /// </summary>
        [Required]
        public UserCard Card { get; set; }
        /// <summary>
        /// Favourit books of user
        /// </summary>
        public List<Book> Books { get; set; }

        public User() { }

        public User(string login, string password)
        {
            Login = login;
            Password = password;

            Card = new UserCard();
            Books = new List<Book>();
        }
    }
}
