using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Core;
using Library.MVVM.Model;
using System.Collections.ObjectModel;

namespace Library.MVVM.ViewModel
{
    class BooksViewModel : ObservableObject
    {
        #region Singleton
        private static BooksViewModel instance;

        public static BooksViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BooksViewModel();
                }
                return instance;
            }
            set => instance = value;
        }
        #endregion

        //Data
        private ObservableCollection<Tag> baseTagList = new ObservableCollection<Tag>();
        private readonly ObservableCollection<Tag> emptyTagList = new ObservableCollection<Tag>();
        private List<Book> baseBookList = new List<Book>();
        public User Account { get; set; }

        //Commands
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }

        //Bindable properties
        #region BookSearch
        private string _BookSearch;

        public string BookSearch
        {
            get { return _BookSearch; }
            set
            {
                _BookSearch = value;
                OnPropertyChanged();
                Task.Run(ChangeBookList);
            }
        }
        #endregion

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

        #region SelTag
        private Tag _SelTag;

        public Tag SelTag
        {
            get { return _SelTag; }
            set
            {
                _SelTag = value;
                OnPropertyChanged();
                if (TagList.Contains(SelTag) && !FilterTagList.Contains(SelTag))
                {
                    FilterTagList.Add(SelTag);
                    //MessageBox.Show(FilterTagList.Count.ToString());
                    Task.Run(ChangeBookList);
                    TagList.Remove(SelTag);
                    SelTag = null;
                }
            }
        }
        #endregion

        #region FilterTagList
        private ObservableCollection<Tag> _FilterTagList = new ObservableCollection<Tag>();

        public ObservableCollection<Tag> FilterTagList
        {
            get { return _FilterTagList; }
            set
            {
                _FilterTagList = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region FilterSelTag
        private Tag _FilterSelTag;

        public Tag FilterSelTag
        {
            get { return _FilterSelTag; }
            set
            {
                _FilterSelTag = value;
                OnPropertyChanged();
                if (!TagList.Contains(FilterSelTag) && FilterTagList.Contains(FilterSelTag))
                {
                    TagList.Add(FilterSelTag);
                    FilterTagList.Remove(FilterSelTag);
                    FilterSelTag = null;
                }
                Task.Run(ChangeBookList);
            }
        }
        #endregion

        #region SortIndex
        private int? _SortIndex;

        public int? SortIndex
        {
            get { return _SortIndex; }
            set
            {
                _SortIndex = value;
                OnPropertyChanged();
                Task.Run(ChangeBookList);
            }
        }
        #endregion

        #region IsSearching
        private bool _FavsCheck;

        public bool FavsCheck
        {
            get { return _FavsCheck; }
            set
            {
                _FavsCheck = value;
                OnPropertyChanged();
                Task.Run(ChangeBookList);
                //ChangeBookList();
            }
        }
        #endregion

        #region IsSearching
        private bool _IsSearching = false;

        public bool IsSearching
        {
            get { return _IsSearching; }
            set
            {
                _IsSearching = value;
                OnPropertyChanged();
            }
        }
        #endregion

        bool TagsCheck(Book book)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                foreach (Tag tag in FilterTagList)
                {
                    bool flag = false;
                    foreach (Tag tag1 in book.Tags)
                    {
                        if (tag.Name == tag1.Name)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                        return false;
                }
                return true;

            }
        }

        void ChangeBookList()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                Account = db.Users.Include("Books").FirstOrDefault(u => u.UserId == Account.UserId);
                BookList = db.Books.Include("Tags").OrderBy(b => b.BookId).ToList();
                TagList = new ObservableCollection<Tag>(TagList.OrderBy(t => t.Name).ToList());

                if (FavsCheck)
                    BookList = Account.Books.ToList();
                if (!string.IsNullOrEmpty(BookSearch))
                    BookList = BookList.Where(b => b.Name.Trim().Contains(BookSearch.Trim())).ToList();
                if (FilterTagList.Count != 0)
                    BookList = BookList.Where(b => TagsCheck(b)).ToList();
                if (SortIndex != null)
                    switch (SortIndex)
                    {
                        case 0:
                            {
                                BookList = BookList.OrderBy(b => b.Name).ToList();
                                break;
                            }
                        case 1:
                            {
                                BookList = BookList.OrderBy(b => b.Author).ToList();
                                break;
                            }
                        case 2:
                            {
                                BookList = BookList.OrderBy(b => b.Downloads).ToList();
                                break;
                            }
                        default:
                            break;
                    }
                foreach (Book bk in Account.Books)
                {
                    if (BookList.FirstOrDefault(b => b == bk) != null)
                        BookList.FirstOrDefault(b => b.Name == bk.Name).InFavs = true;
                }
            }
        }

        public BooksViewModel() { }

        public BooksViewModel(User user)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                FilterTagList = emptyTagList;
                Account = db.Users.Include("Books").FirstOrDefault(u => u.UserId == user.UserId);
                TagList = baseTagList = new ObservableCollection<Tag>(db.Tags.OrderBy(t => t.Name).ToList());
                BookList = baseBookList = db.Books.Include("Tags").OrderBy(b => b.BookId).ToList();
                foreach (Book bk in Account.Books)
                {
                    BookList.FirstOrDefault(b => b == bk).InFavs = true;
                }
            }

            #region Reset
            ResetCommand = new RelayCommand(o =>
            {
                FilterTagList = new ObservableCollection<Tag>();
                BookSearch = "";
                SortIndex = null;
                FavsCheck = false;
                TagList = baseTagList;
                BookList = baseBookList;
            });
            #endregion

            #region Refreshs
            RefreshCommand = new RelayCommand(o =>
                {
                    ChangeBookList();
                }); 
            #endregion

            Instance = this;
        }

    }
}
