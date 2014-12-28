using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MyWMPv2.Utilities;

namespace MyWMPv2.Model
{
    class LibraryVideo : ObservableObject
    {
        private readonly string[] _extensions = { ".avi", ".mp4", ".mkv" };
        private String _directory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        private List<MyVideo> _items;

        public LibraryVideo()
        {
            _items = new List<MyVideo>();   
        }

        public String Directory
        {
            get { return _directory; }
            set { _directory = value; }
        }
        public List<MyVideo> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged("ItemsVideo");
            }
        }

        public void Refresh()
        {
            Items.Clear();
            string[] files = System.IO.Directory.GetFiles(_directory, "*.*", SearchOption.AllDirectories);
            Items = (from file in files
                      where _extensions.Any(Path.GetExtension(file).Contains)
                      let tagFile = TagLib.File.Create(file)
                      select new MyVideo()
                      {
                          Path = file,
                          Filename = Path.GetFileNameWithoutExtension(file),
                          Height = tagFile.Properties.VideoHeight,
                          Width = tagFile.Properties.VideoWidth,
                          Length = tagFile.Properties.Duration
                      }).ToList();
        }
    }
}
