using System;
using System.Data.Entity;

namespace Library.MVVM.Model
{
    class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DefaultConnection")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        //public DbSet<FavBook> FavBooks { get; set; }
    }
}
