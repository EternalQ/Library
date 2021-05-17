using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.MVVM.Model
{
    public class Book
    {
        public int BookId { get; set; }

        //public string ImagePath { get; set; }
        public byte[] Image { get; set; }
        public byte[] DataFB2 { get; set; }
        public byte[] DataEPUB { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }

        public List<Tag> Tags { get; set; }
        public List<User> Users { get; set; }

        public Book() { }

        public Book(string name, string author, string description, byte[] image)
        {
            Name = name;
            Author = author;
            Description = description;
            Image = image;

            Tags = new List<Tag>();
            Users = new List<User>();
        }
    }
}
