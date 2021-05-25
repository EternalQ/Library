using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.MVVM.Model
{
    public class UserCard
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }

        public User User { get; set; }
    }
}
