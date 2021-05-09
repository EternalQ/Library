using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.MVVM.Model
{
    class Book
    {
        public int BookId { get; set; }

        //public byte[] Image { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }

        public List<User> users { get; set; }
    }
}
