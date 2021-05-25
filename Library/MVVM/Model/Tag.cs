using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.MVVM.Model
{
    public class Tag
    {
        public Tag() { }

        public Tag(string name)
        {
            Name = name;
        }

        public int TagId { get; set; }

        public string Name { get; set; }
        [NotMapped] 
        public bool IsChecked { get; set; }

        public List<Book> Books { get; set; }
    }
}
