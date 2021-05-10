using System;
using System.Data.Entity;

namespace Library.MVVM.Model
{
    class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DefaultConnection")
        { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCard> UserCards { get; set; }
    }
}
