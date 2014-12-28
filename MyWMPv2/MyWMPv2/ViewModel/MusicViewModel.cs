using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private LibraryMusic _library;
        private String _itemSelected;

        public MusicViewModel()
        {
            _library = new LibraryMusic();
            _library.PropertyChanged += PropertyChangedHandler;
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

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

        /*
         * Event Handler
         */
        public void Music_Click(object sender, RoutedEventArgs e,
            ListView listMusic, ListView listVideo, ListView listImage)
        {
            if (listMusic.Visibility == Visibility.Visible)
                listMusic.Visibility = Visibility.Collapsed;
            else if (listMusic.Visibility == Visibility.Collapsed)
                listMusic.Visibility = Visibility.Visible;
            listVideo.Visibility = Visibility.Collapsed;
            listImage.Visibility = Visibility.Collapsed;
            _library.Refresh();
        }
        public void Music_Directory(object sender, RoutedEventArgs routedEventArgs, ListView listMusic)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK) return;
            _library.Directory = fbd.SelectedPath;
            _library.Refresh();
        }
        public void ListMusic_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            SortLibrary.SortColumn(routedEventArgs, _library.Items);
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
    }
}
