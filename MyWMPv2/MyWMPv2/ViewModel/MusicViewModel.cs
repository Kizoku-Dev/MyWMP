﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using MyWMPv2.Model;
using MyWMPv2.Utilities;
using MyWMPv2.View;
using ListView = System.Windows.Controls.ListView;
using ListViewItem = System.Windows.Controls.ListViewItem;

namespace MyWMPv2.ViewModel
{
    class MusicViewModel : ObservableObject
    {
        #region Private member variables
        private LibraryMusic _library;
        private String _itemSelected;
        #endregion Private member variables

        public MusicViewModel()
        {
            _library = new LibraryMusic();
            _library.PropertyChanged += PropertyChangedHandler;
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        #region Public member variables
        public LibraryMusic Library
        {
            get { return _library; }
            set { _library = value; }
        }

        public String ItemSelected
        {
            get { return _itemSelected; }
            set { _itemSelected = value; }
        }
        #endregion Public member variables

        #region Event Handler
        /*
         * Event Handler
         */
        public void Music_Click(object sender, RoutedEventArgs e,
            ListView listMusic, ListView listVideo, ListView listImage, String fgList)
        {
            if (listMusic.Visibility == Visibility.Visible)
                listMusic.Visibility = Visibility.Collapsed;
            else if (listMusic.Visibility == Visibility.Collapsed)
                listMusic.Visibility = Visibility.Visible;
            listVideo.Visibility = Visibility.Collapsed;
            listImage.Visibility = Visibility.Collapsed;
            _library.Refresh(fgList);
        }
        public void Music_Directory(object sender, RoutedEventArgs routedEventArgs, ListView listMusic, String fgList)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK) return;
            _library.Directory = fbd.SelectedPath;
            _library.Refresh(fgList);
        }
        public void Music_SearchFilename(String fgList)
        {
            String search = MyDialog.Prompt("Custom search", "Enter the string to search", MyDialog.Size.Big);
            _library.ApplySearchFilename(search);
        }
        public void Music_SearchAuthor(String fgList)
        {
            String search = MyDialog.Prompt("Custom search", "Enter the string to search", MyDialog.Size.Big);
            _library.ApplySearchAuthor(search);
        }
        public void Music_SearchAlbum(String fgList)
        {
            String search = MyDialog.Prompt("Custom search", "Enter the string to search", MyDialog.Size.Big);
            _library.ApplySearchAlbum(search);
        }
        public void ListMusic_Click(object sender, RoutedEventArgs e, List<ThemeElem> theme)
        {
            SortLibrary.SortColumn(e, _library.Items, theme);
        }

        public void ListMusic_DoubleClick(object sender, MouseButtonEventArgs e,
            ListView list)
        {
            DependencyObject dep = (DependencyObject) e.OriginalSource;
            while ((dep != null) && !(dep is ListViewItem))
                dep = VisualTreeHelper.GetParent(dep);
            if (dep == null)
                return;
            MyMusic item = (MyMusic) list.ItemContainerGenerator.ItemFromContainer(dep);
            _itemSelected = item.Path;
            list.Visibility = Visibility.Collapsed;
            OnPropertyChanged("MusicDoubleClick");
        }
        #endregion Event Handler
    }
}
