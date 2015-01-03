using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MyWMPv2.Utilities;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using File = TagLib.File;

namespace MyWMPv2.Model
{
    class LibraryMusic : ObservableObject
    {
        private readonly string[] _extensions = { ".mp3", ".wav", ".ogg", ".flac" };
        private String _directory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        private List<MyMusic> _items;

        public LibraryMusic()
        {
            _items = new List<MyMusic>();   
        }

        public String Directory
        {
            get { return _directory; }
            set { _directory = value; }
        }
        public List<MyMusic> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged("ItemsMusic");
            }
        }

        public void Refresh(String fgList)
        {
            Items.Clear();
            string[] files = System.IO.Directory.GetFiles(_directory, "*.*", SearchOption.AllDirectories);
            try
            {
            Items = (from file in files
                      where _extensions.Any(Path.GetExtension(file).Contains)
                      let tagFile = File.Create(file)
                      select new MyMusic()
                      {
                          Path = file,
                          Filename = Path.GetFileNameWithoutExtension(file),
                          Title = tagFile.Tag.Title,
                          Album = tagFile.Tag.Album,
                          Author = tagFile.Tag.FirstAlbumArtist,
                          Length = tagFile.Properties.Duration,
                          FgList = Converter.StringToColor(fgList)
                      }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error taglib");
                MessageBox.Show(e.Message, "Error taglib", MessageBoxButton.OK);
            }
        }
    }
}
