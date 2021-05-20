﻿using Library.Core;
using Library.MVVM.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Library.MVVM.ViewModel
{
    class AddBooksViewModel : ObservableObject
    {
        public RelayCommand AddBook { get; set; }
        public RelayCommand Cancel { get; set; }
        public RelayCommand AddImage { get; set; }
        public RelayCommand AddFB2 { get; set; }
        public RelayCommand AddEPUB { get; set; }

        private byte[] imgData;
        private byte[] fb2Data;
        private byte[] epubData;
        private ObservableCollection<Tag> baseTagList;

        #region ImageSource
        private string _ImageSource = "Source";

        public string ImageSource
        {
            get { return _ImageSource; }
            set
            {
                _ImageSource = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region FB2Source
        private string _FB2Source = "Source";

        public string FB2Source
        {
            get { return _FB2Source; }
            set
            {
                _FB2Source = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region EPUBSource
        private string _EPUBSource = "Source";

        public string EPUBSource
        {
            get { return _EPUBSource; }
            set
            {
                _EPUBSource = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Name
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                ClearErrors();
                if (_Name == "")
                    AddError("Can't be empty");
                OnPropertyChanged();
            }
        }
        #endregion

        #region AuthorName
        private string _AuthorName;

        public string AuthorName
        {
            get { return _AuthorName; }
            set
            {
                _AuthorName = value;
                ClearErrors();
                if (_AuthorName == "")
                    AddError("Can't be empty");
                OnPropertyChanged();
            }
        }
        #endregion

        #region Description
        private string _Description;

        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                ClearErrors();
                if (_Description == "")
                    AddError("Can't be empty");
                OnPropertyChanged();
            }
        }
        #endregion

        #region DataError
        private string _DataError;

        public string DataError
        {
            get { return _DataError; }
            set
            {
                _DataError = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region TagSearch
        private string _TagSearch;

        public string TagSearch
        {
            get { return _TagSearch; }
            set
            {
                _TagSearch = value;
                OnPropertyChanged();
                using (DatabaseContext db = new DatabaseContext())
                {
                    if (TagSearch != "")
                        TagList = new ObservableCollection<Tag>(db.Tags.Where(t => t.Name.Trim().Contains(TagSearch.Trim())).ToList());
                    else
                        TagList = baseTagList;
                }
            }
        }
        #endregion

        #region ImageError
        private string _ImageError;

        public string ImageError
        {
            get { return _ImageError; }
            set
            {
                _ImageError = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region TagList
        private ObservableCollection<Tag> _TagList;

        public ObservableCollection<Tag> TagList
        {
            get { return _TagList; }
            set
            {
                _TagList = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region SelectedTag
        private Tag _SelectedTag;

        public Tag SelectedTag
        {
            get { return _SelectedTag; }
            set
            {
                _SelectedTag = value;
                OnPropertyChanged();
                //if (BookTags.Contains(SelectedTag))
                //    MessageBox.Show("he");
                if (TagList.Contains(SelectedTag) && !BookTags.Contains(SelectedTag))
                {
                    BookTags.Add(SelectedTag);
                    //SelectedTag = null;
                    TagList.Remove(SelectedTag);
                }
            }
        }
        #endregion

        #region BookTags
        private ObservableCollection<Tag> _BookTags;

        public ObservableCollection<Tag> BookTags
        {
            get { return _BookTags; }
            set
            {
                _BookTags = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region SelectedBookTag
        private Tag _SelectedBookTag;

        public Tag SelectedBookTag
        {
            get { return _SelectedBookTag; }
            set
            {
                _SelectedBookTag = value;
                OnPropertyChanged();
                if (BookTags.Contains(SelectedBookTag) && !TagList.Contains(SelectedBookTag))
                {
                    TagList.Add(SelectedBookTag);
                    BookTags.Remove(SelectedBookTag);
                }
            }
        }
        #endregion

        public AddBooksViewModel(Book newbook)
        {
            if (newbook != null)
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    newbook = db.Books.Include("Tags").FirstOrDefault(b => b.BookId == newbook.BookId);
                }
                Name = newbook.Name;
                AuthorName = newbook.Author;
                Description = newbook.Description;
                BookTags = new ObservableCollection<Tag>(newbook.Tags);
                if (newbook.Image != null)
                {
                    ImageSource = "Uploaded";
                    imgData = newbook.Image;
                }
                if (newbook.DataFB2 != null)
                {
                    FB2Source = "Uploaded";
                    fb2Data = newbook.DataFB2;
                }
                if (newbook.DataEPUB != null)
                {
                    EPUBSource = "Uploaded";
                    epubData = newbook.DataEPUB;
                }
            }

            using (DatabaseContext db = new DatabaseContext())
            {
                BookTags = new ObservableCollection<Tag>();
                baseTagList = new ObservableCollection<Tag>(db.Tags.OrderBy(t => t.Name).ToList());
                TagList = baseTagList;
            }

            //add rewriting
            #region AddBook
            AddBook = new RelayCommand(o =>
            {
                if (string.IsNullOrWhiteSpace(ImageSource))
                {
                    ImageError = "Add Image";
                    return;
                }

                if (fb2Data == null && epubData == null)
                {
                    DataError = "Add some of data sources";
                    return;
                }

                Book book = new Book(Name, AuthorName, Description, imgData, fb2Data, epubData);

                using (DatabaseContext db = new DatabaseContext())
                {
                    foreach (Tag tag in BookTags.ToList())
                    {
                        book.Tags.Add(db.Tags.FirstOrDefault(t => t.TagId == tag.TagId));
                    }

                    db.Books.Add(book);
                    db.SaveChanges();
                    MessageBox.Show(book.Tags.Count.ToString());
                }
            }, o =>
             {
                 if (String.IsNullOrWhiteSpace(Name) || String.IsNullOrWhiteSpace(AuthorName) ||
                 String.IsNullOrWhiteSpace(Description))
                     return false;
                 return true;
             });
            #endregion

            #region AddImg
            AddImage = new RelayCommand(o =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Add Image";
                ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
                if (ofd.ShowDialog() == true)
                {
                    ImageSource = ofd.FileName;
                    var fs = ofd.OpenFile();
                    imgData = new byte[fs.Length];
                    fs.Read(imgData, 0, imgData.Length);
                }
            });
            #endregion

            #region Addfb2
            AddFB2 = new RelayCommand(o =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Add FB2";
                ofd.Filter = "Image Files(*.FB2)|*.FB2|All files (*.*)|*.*";
                if (ofd.ShowDialog() == true)
                {
                    FB2Source = ofd.FileName;
                    var fs = ofd.OpenFile();
                    fb2Data = new byte[fs.Length];
                    fs.Read(fb2Data, 0, fb2Data.Length);
                }
            });
            #endregion

            #region AddEPUB
            AddEPUB = new RelayCommand(o =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Add EPUB";
                ofd.Filter = "Image Files(*.EPUB)|*.EPUB|All files (*.*)|*.*";
                if (ofd.ShowDialog() == true)
                {
                    EPUBSource = ofd.FileName;
                    var fs = ofd.OpenFile();
                    epubData = new byte[fs.Length];
                    fs.Read(epubData, 0, epubData.Length);
                }
            });
            #endregion

            #region Cancel
            Cancel = new RelayCommand(o =>
            {
                AdminWindViewModel.Instance.ToBooksCommand.Execute(null);
            });
            #endregion
        }
    }
}