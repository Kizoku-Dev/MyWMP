using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using MyWMPv2.Utilities;
using File = TagLib.File;

namespace MyWMPv2.Model
{
    class LibraryMusic : ObservableObject
    {
        #region Private member variables
        private readonly string[] _extensions = { ".mp3", ".wav", ".ogg", ".flac" };
        private String _directory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        private List<MyMusic> _items;
        #endregion Private member variables

        public LibraryMusic()
        {
            _items = new List<MyMusic>();   
        }

        #region Public member variables
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
        #endregion Public member variables

        public void Refresh(String fgList)
        {
            Items.Clear();
            try
            {
                string[] files = System.IO.Directory.GetFiles(_directory, "*.*", SearchOption.AllDirectories);
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
                Console.WriteLine("Error refreshing");
                MessageBox.Show(e.Message + "\n\nPlease, fix this error before continuing.", "Error refreshing", MessageBoxButton.OK);
            }
        }

        public void ApplySearchFilename(String search)
        {
            Items = Items.Where(item => item.Filename.Contains(search)).ToList();
        }
        public void ApplySearchAuthor(String search)
        {
            Items.RemoveAll(item => item.Author == null);
            Items = Items.Where(item => item.Author.Contains(search)).ToList();
        }
        public void ApplySearchAlbum(String search)
        {
            Items.RemoveAll(item => item.Album == null);
            Items = Items.Where(item => item.Album.Contains(search)).ToList();
        }
    }
}
