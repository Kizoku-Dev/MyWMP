using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using MyWMPv2.Utilities;
using File = TagLib.File;

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
            try
            {
                Items = (from file in files
                         where _extensions.Any(Path.GetExtension(file).Contains)
                         let tagFile = File.Create(file)
                         select new MyVideo()
                         {
                             Path = file,
                             Filename = Path.GetFileNameWithoutExtension(file),
                             Height = tagFile.Properties.VideoHeight,
                             Width = tagFile.Properties.VideoWidth,
                             Length = tagFile.Properties.Duration
                         }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error taglib");
                MessageBox.Show(e.Message+"\n\nPlease, fix this error before continuing.", "Error taglib", MessageBoxButton.OK);
            }
        }
    }
}
