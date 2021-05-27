using Library.Core;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        public int Downloads { get; set; }
        [NotMapped]
        public bool InFavs { get; set; }

        #region TagsAsString
        [NotMapped]
        public string TagsTxt { get => GetTags(); }

        public string GetTags()
        {
            string tags = "Tags: ";

            if (tags != null)
                foreach (Tag tag in Tags)
                    tags += $"{tag.Name} ";

            return tags;
        }
        #endregion

        public List<Tag> Tags { get; set; }
        public List<User> Users { get; set; }

        #region DownloadCommands
        public RelayCommand DownloadFB2Command
        {
            get => new RelayCommand(o =>
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = $"Save {Name} as FB2";
                sfd.FileName = $"{Author}_{Name}.fb2";
                sfd.Filter = "FB2 Files(*.FB2)|*.FB2|All files (*.*)|*.*";
                if (sfd.ShowDialog() == true)
                {
                    var fs = sfd.OpenFile();
                    fs.Write(DataFB2, 0, DataFB2.Length);
                    using (DatabaseContext db = new DatabaseContext())
                    {
                        db.Books.FirstOrDefault(b => b.Name == this.Name).Downloads += 1;
                        db.SaveChanges();
                    }
                }
            }, o =>
            {
                if (DataFB2 == null)
                    return false;
                else
                    return true;
            });
        }

        public RelayCommand DownloadEPUBCommand
        {
            get => new RelayCommand(o =>
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = $"Save {Name} as FB2";
                sfd.FileName = $"{Author}_{Name}.epub";
                sfd.Filter = "EPUB Files(*.EPUB)|*.EPUB|All files (*.*)|*.*";
                if (sfd.ShowDialog() == true)
                {
                    var fs = sfd.OpenFile();
                    fs.Write(DataFB2, 0, DataFB2.Length);
                    using (DatabaseContext db = new DatabaseContext())
                    {
                        db.Books.FirstOrDefault(b => b.Name == this.Name).Downloads += 1;
                        db.SaveChanges();
                    }
                }
            }, o =>
            {
                if (DataEPUB == null)
                    return false;
                else
                    return true;
            });
        }
        #endregion

        public Book() { }

        public Book(string name, string author, string description, byte[] image, byte[] dataFB2, byte[] dataEPUB)
        {
            Name = name;
            Author = author;
            Description = description;
            Image = image;
            DataFB2 = dataFB2;
            DataEPUB = dataEPUB;

            Tags = new List<Tag>();
            Users = new List<User>();
        }
    }
}
