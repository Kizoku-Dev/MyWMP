using System;
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
    class VideoViewModel : ObservableObject
    {
        #region Private member variables
        private LibraryVideo _library;
        private String _itemSelected;
        #endregion Private member variables

        public VideoViewModel()
        {
            _library = new LibraryVideo();
            _library.PropertyChanged += PropertyChangedHandler;
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        #region Public member variables
        public LibraryVideo Library
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
        public void Video_Click(object sender, RoutedEventArgs e,
            ListView listMusic, ListView listVideo, ListView listImage, String fgList)
        {
            if (listVideo.Visibility == Visibility.Visible)
                listVideo.Visibility = Visibility.Collapsed;
            else if (listVideo.Visibility == Visibility.Collapsed)
                listVideo.Visibility = Visibility.Visible;
            listMusic.Visibility = Visibility.Collapsed;
            listImage.Visibility = Visibility.Collapsed;
            _library.Refresh(fgList);
        }
        public void Video_Directory(object sender, RoutedEventArgs routedEventArgs, ListView listMusic, String fgList)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK) return;
            _library.Directory = fbd.SelectedPath;
            _library.Refresh(fgList);
        }
        public void Video_Search(String fgList)
        {
            String search = MyDialog.Prompt("Custom search", "Enter the string to search", MyDialog.Size.Big);
            _library.ApplySearch(search);
        }
        public void ListVideo_Click(object sender, RoutedEventArgs e, List<ThemeElem> theme)
        {
            SortLibrary.SortColumn(e, _library.Items, theme);
        }

        public void ListVideo_DoubleClick(object sender, MouseButtonEventArgs e,
            ListView list)
        {
            DependencyObject dep = (DependencyObject) e.OriginalSource;
            while ((dep != null) && !(dep is ListViewItem))
                dep = VisualTreeHelper.GetParent(dep);
            if (dep == null)
                return;
            MyVideo item = (MyVideo)list.ItemContainerGenerator.ItemFromContainer(dep);
            _itemSelected = item.Path;
            list.Visibility = Visibility.Collapsed;
            OnPropertyChanged("VideoDoubleClick");
        }
        #endregion Event Handler
    }
}
