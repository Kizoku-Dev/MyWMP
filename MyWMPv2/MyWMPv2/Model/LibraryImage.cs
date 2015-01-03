using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using MyWMPv2.Utilities;

namespace MyWMPv2.Model
{
    class LibraryImage : ObservableObject
    {
        private readonly string[] _extensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        private String _directory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        private List<MyImage> _items;

        public LibraryImage()
        {
            _items = new List<MyImage>();   
        }

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

        public void Refresh(String fgList)
        {
            Items.Clear();
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
    }
}
