using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using MyWMPv2.Model;
using MyWMPv2.Utilities;
using ListView = System.Windows.Controls.ListView;
using ListViewItem = System.Windows.Controls.ListViewItem;

namespace MyWMPv2.ViewModel
{
    class ImageViewModel : ObservableObject
    {
        private LibraryImage _library;
        private String _itemSelected;

        public ImageViewModel()
        {
            _library = new LibraryImage();
            _library.PropertyChanged += PropertyChangedHandler;
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public LibraryImage Library
        {
            get { return _library; }
            set { _library = value; }
        }

        public String ItemSelected
        {
            get { return _itemSelected; }
            set { _itemSelected = value; }
        }

        /*
         * Event Handler
         */
        public void Image_Click(object sender, RoutedEventArgs e,
            ListView listMusic, ListView listVideo, ListView listImage, String fgList)
        {
            if (listImage.Visibility == Visibility.Visible)
                listImage.Visibility = Visibility.Collapsed;
            else if (listImage.Visibility == Visibility.Collapsed)
                listImage.Visibility = Visibility.Visible;
            listMusic.Visibility = Visibility.Collapsed;
            listVideo.Visibility = Visibility.Collapsed;
            _library.Refresh(fgList);
        }
        public void Image_Directory(object sender, RoutedEventArgs routedEventArgs, ListView listMusic, String fgList)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK) return;
            _library.Directory = fbd.SelectedPath;
            _library.Refresh(fgList);
        }
        public void ListImage_Click(object sender, RoutedEventArgs e, List<ThemeElem> theme)
        {
            SortLibrary.SortColumn(e, _library.Items, theme);
        }

        public void ListImage_DoubleClick(object sender, MouseButtonEventArgs e,
            ListView list)
        {
            DependencyObject dep = (DependencyObject) e.OriginalSource;
            while ((dep != null) && !(dep is ListViewItem))
                dep = VisualTreeHelper.GetParent(dep);
            if (dep == null)
                return;
            MyImage item = (MyImage)list.ItemContainerGenerator.ItemFromContainer(dep);
            _itemSelected = item.Path;
            list.Visibility = Visibility.Collapsed;
            OnPropertyChanged("ImageDoubleClick");
        }
    }
}
