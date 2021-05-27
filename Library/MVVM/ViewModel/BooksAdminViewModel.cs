using System.Collections.Generic;
using System.Linq;
using Library.Core;
using Library.MVVM.Model;

namespace Library.MVVM.ViewModel
{
    class BooksAdminViewModel : ObservableObject
    {
        #region Singleton
        private static BooksAdminViewModel instance;

        public static BooksAdminViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BooksAdminViewModel();
                }
                return instance;
            }
            set => instance = value;
        }
        #endregion

        //Commands
        public RelayCommand DeleteSelectedBook { get; set; }
        public RelayCommand SaveBookInfo { get; set; }
        public RelayCommand DeleteSelectedTag { get; set; }
        public RelayCommand AddTag { get; set; }
        public RelayCommand OpenDialog { get; set; }
        public RelayCommand AddBook { get; set; }
        public RelayCommand BookEdit { get; set; }

        //data
        string name;

        //Bindable properties
        #region BookList
        private List<Book> _BookList;

        public List<Book> BookList
        {
            get { return _BookList; }
            set
            {
                _BookList = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region SelectedBook
        private Book _SelectedBook;

        public Book SelectedBook
        {
            get { return _SelectedBook; }
            set
            {
                _SelectedBook = value;
                OnPropertyChanged();
                ChangeBookInfo();
            }
        }

        private void ChangeBookInfo()
        {
            Book selected;
            List<Tag> tags;
            using (DatabaseContext db = new DatabaseContext())
            {

                selected = db.Books.Include("Tags").FirstOrDefault(b => b.BookId == SelectedBook.BookId);
                tags = selected.Tags;
            }

            Name = name = selected.Name;
            AuthorName = selected.Author;
            Description = selected.Description;
            SelectedBookTags = tags;
            IsBookSelected = true;
        }
        #endregion

        #region SelectedBookTags
        private List<Tag> _SelectedBookTags;

        public List<Tag> SelectedBookTags
        {
            get { return _SelectedBookTags; }
            set
            {
                _SelectedBookTags = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region TagList
        private List<Tag> _TagList;

        public List<Tag> TagList
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
                using (DatabaseContext db = new DatabaseContext())
                {
                    if (db.Books.FirstOrDefault(b => b.Name == _Name && Name != name) != null)
                        AddError("Book will be rewrited");
                }
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

        #region TagName
        private string _TagName;

        public string TagName
        {
            get { return _TagName; }
            set
            {
                _TagName = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region BookSearch
        private string _BookSearch;

        public string BookSearch
        {
            get { return _BookSearch; }
            set
            {
                _BookSearch = value;
                OnPropertyChanged();
                using (DatabaseContext db = new DatabaseContext())
                {
                    if (BookSearch != "")
                        BookList = db.Books.Where(b => b.Name.Trim().Contains(value.Trim())).ToList();
                    else
                        BookList = db.Books.ToList();
                }
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
                        TagList = db.Tags.Where(t => t.Name.Trim().Contains(TagSearch.Trim())).ToList();
                    else
                        TagList = db.Tags.ToList();
                }
            }
        }
        #endregion

        #region IsFB2
        private bool _IsFB2;

        public bool IsFB2
        {
            get { return _IsFB2; }
            set
            {
                _IsFB2 = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region IsEPUB
        private bool _IsEPUB;

        public bool IsEPUB
        {
            get { return _IsEPUB; }
            set
            {
                _IsEPUB = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region IsOpen
        private bool _IsOpen;

        public bool IsOpen
        {
            get { return _IsOpen; }
            set
            {
                _IsOpen = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Selection
        private bool _IsBookSelected;

        public bool IsBookSelected
        {
            get { return _IsBookSelected; }
            set
            {
                _IsBookSelected = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public BooksAdminViewModel() { }

        public BooksAdminViewModel(Book tbook)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                BookList = db.Books.Include("Tags").ToList();
                if (tbook != null)
                    BookEdit.Execute(SelectedBook);

                TagList = db.Tags.ToList();
            }

            #region SaveBookInfo
            SaveBookInfo = new RelayCommand(o =>
            {
                using(DatabaseContext db = new DatabaseContext())
                {
                    db.Books.FirstOrDefault(b => b.BookId == SelectedBook.BookId).Name = Name;
                    db.Books.FirstOrDefault(b => b.BookId == SelectedBook.BookId).Author = AuthorName;
                    db.Books.FirstOrDefault(b => b.BookId == SelectedBook.BookId).Description = Description;
                    db.SaveChanges();
                }
            });
            #endregion

            #region DeleteSelectedBook
            DeleteSelectedBook = new RelayCommand(o =>
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    db.Books.Remove(db.Books.FirstOrDefault(b => b.BookId == SelectedBook.BookId));
                    db.SaveChanges();
                    BookList = db.Books.ToList();
                }
            }, o =>
            {
                if (SelectedBook == null)
                    return false;
                else
                    return true;
            });
            #endregion

            #region DeleteSelectedTag
            DeleteSelectedTag = new RelayCommand(o =>
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    db.Tags.Remove(db.Tags.FirstOrDefault(t => t.TagId == SelectedTag.TagId));
                    db.SaveChanges();
                    TagList = db.Tags.ToList();
                }
            }, o =>
            {
                if (SelectedTag == null)
                    return false;
                else
                    return true;
            });
            #endregion

            #region AddTag
            AddTag = new RelayCommand(o =>
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    if (db.Tags.Any(t => t.Name == TagName))
                        AddError("This tag alredy exists", nameof(TagName));
                    else
                    {
                        db.Tags.Add(new Tag(TagName));
                        db.SaveChanges();
                        TagList = db.Tags.ToList();
                        IsOpen = false;
                    }
                }
            });
            #endregion

            #region OpenDialog
            OpenDialog = new RelayCommand(o =>
            {
                IsOpen = true;
            });
            #endregion

            #region AddBook
            AddBook = new RelayCommand(o =>
            {
                AdminWindViewModel.Instance.CurrentView = new AddBooksViewModel(null);
            });
            #endregion

            #region BookEdit
            BookEdit = new RelayCommand(o =>
            {
                Book newbook;
                if (o != null)
                {
                    newbook = (Book)o;
                }
                else
                {
                    newbook = SelectedBook;
                }

                using (DatabaseContext db = new DatabaseContext())
                    AdminWindViewModel.Instance.CurrentView = new AddBooksViewModel(db.Books.FirstOrDefault(b => b.Name == newbook.Name));
            });
            #endregion

            Instance = this;
        }
    }
}
