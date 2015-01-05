using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using MyWMPv2.Utilities;

namespace MyWMPv2.Model
{
    class LibraryImage : ObservableObject
    {
        #region Private member variables
        private readonly string[] _extensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        private String _directory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        private List<MyImage> _items;
        #endregion Private member variables

        public LibraryImage()
        {
            _items = new List<MyImage>();   
        }

        #region Public member variables
        public String Directory
        {
            get { return _directory; }
            set { _directory = value; }
        }
        public List<MyImage> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged("ItemsImage");
            }
        }
        #endregion Public member variables

        public void Refresh(String fgList)
        {
            Items.Clear();
            try
            {
                string[] files = System.IO.Directory.GetFiles(_directory, "*.*", SearchOption.AllDirectories);
                Items = (from file in files
                         where _extensions.Any(Path.GetExtension(file).Contains)
                         select new MyImage()
                         {
                             Path = file,
                             Filename = Path.GetFileNameWithoutExtension(file),
                             FgList = Converter.StringToColor(fgList)
                         }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error refreshing");
                MessageBox.Show(e.Message + "\n\nPlease, fix this error before continuing.", "Error refreshing", MessageBoxButton.OK);
            }
        }

        public void ApplySearch(String search)
        {
            Items = Items.Where(item => item.Filename.Contains(search)).ToList();
        }
    }
}
