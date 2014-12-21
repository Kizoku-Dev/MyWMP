using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WMP
{
    class Library
    {
        private String _musicDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        private String _videoDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        private String _imageDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        readonly string[] _musicExtensions = { ".mp3", ".wav", ".ogg", ".flac" };
        readonly string[] _videoExtensions = { ".avi", ".mp4", ".mkv" };
        readonly string[] _imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        /*
         * Directory path setters
         */
        public void SetMusicDirectory(String s)
        {
            this._musicDirectory = s;
        }

        public void SetVideoDirectory(String s)
        {
            this._videoDirectory = s;
        }

        public void SetImageDirectory(String s)
        {
            this._imageDirectory = s;
        }

        /*
         * Refresh list with path
         */
        public void RefreshMusic(ListView listLibrary)
        {
            string[] files = Directory.GetFiles(this._musicDirectory, "*.*", SearchOption.AllDirectories);
            List<MyMusic> items = (from file in files
                                   where this._musicExtensions.Any(Path.GetExtension(file).Contains)
                                   let tagFile = TagLib.File.Create(file)
                                   select new MyMusic()
                                   {
                                       Path = file,
                                       Filename = Path.GetFileNameWithoutExtension(file),
                                       Title = tagFile.Tag.Title,
                                       Album = tagFile.Tag.Album,
                                       Author = tagFile.Tag.FirstAlbumArtist,
                                       Length = tagFile.Properties.Duration
                                   }).ToList();
            listLibrary.ItemsSource = items;
        }

        public void RefreshVideo(ListView listLibrary)
        {
            string[] files = Directory.GetFiles(this._videoDirectory, "*.*", SearchOption.AllDirectories);
            List<MyVideo> items = (from file in files
                                   where this._videoExtensions.Any(Path.GetExtension(file).Contains)
                                   let tagFile = TagLib.File.Create(file)
                                   select new MyVideo()
                                   {
                                       Path = file,
                                       Filename = Path.GetFileNameWithoutExtension(file),
                                       Height = tagFile.Properties.VideoHeight,
                                       Width = tagFile.Properties.VideoWidth,
                                       Length = tagFile.Properties.Duration
                                   }).ToList();
            listLibrary.ItemsSource = items;
        }

        public void RefreshImage(ListView listLibrary)
        {
            string[] files = Directory.GetFiles(this._imageDirectory, "*.*", SearchOption.AllDirectories);
            List<MyImage> items = (from file in files
                                   where this._imageExtensions.Any(Path.GetExtension(file).Contains)
                                   select new MyImage()
                                   {
                                       Path = file,
                                       Filename = Path.GetFileNameWithoutExtension(file)
                                   }).ToList();
            listLibrary.ItemsSource = items;
        }

        /*
         * Sorter
         */
        public void SortColumn(RoutedEventArgs e, ListView listLibrary)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;
            if (headerClicked != null && headerClicked.Role != GridViewColumnHeaderRole.Padding)
            {
                if (headerClicked != _lastHeaderClicked)
                    direction = ListSortDirection.Ascending;
                else
                {
                    if (_lastDirection == ListSortDirection.Ascending)
                        direction = ListSortDirection.Descending;
                    else
                        direction = ListSortDirection.Ascending;
                }
                string header = headerClicked.Column.Header as string;
                Sort(header, direction, listLibrary);
                _lastHeaderClicked = headerClicked;
                _lastDirection = direction;
            }
        }
        private void Sort(string sortBy, ListSortDirection direction, ListView listLibrary)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(listLibrary.ItemsSource);
            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }
    }
}
