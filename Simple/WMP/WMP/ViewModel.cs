using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Button = System.Windows.Controls.Button;
using ListView = System.Windows.Controls.ListView;
using ListViewItem = System.Windows.Controls.ListViewItem;
using TreeView = System.Windows.Controls.TreeView;

namespace WMP
{
    class ViewModel
    {
        private readonly Library _library;
        private readonly Playlist _playlist;
        private readonly DispatcherTimer _timer;
        private bool _isDragging = false;

        public ViewModel(Slider sliderMedia, MediaElement media1)
        {
            this._library = new Library();   
            this._playlist = new Playlist();
            this._timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(200)};
            this._timer.Tick += new EventHandler((sender1, e1) => timer_Tick(sliderMedia, media1));
        }

        private void timer_Tick(Slider sliderMedia, MediaElement media1)
        {
            if (!_isDragging)
            {
                sliderMedia.Value = media1.Position.TotalSeconds;
            }

        }

        /*
         * Controller buttons
         */
        public void btnPlay_Click(object sender, RoutedEventArgs e,
            MediaElement media1, Button btnPlay, Button btnPause)
        {
            media1.Play();
            btnPlay.Visibility = Visibility.Collapsed;
            btnPause.Visibility = Visibility.Visible;
        }

        public void btnPause_Click(object sender, RoutedEventArgs e,
            MediaElement media1, Button btnPlay, Button btnPause)
        {
            media1.Pause();
            btnPlay.Visibility = Visibility.Visible;
            btnPause.Visibility = Visibility.Collapsed;
        }

        public void btnStop_Click(object sender, RoutedEventArgs e,
            MediaElement media1, Button btnPlay, Button btnPause)
        {
            media1.Stop();
            btnPlay.Visibility = Visibility.Visible;
            btnPause.Visibility = Visibility.Collapsed;
        }

        public void menuFileOpen_Click(object sender, RoutedEventArgs e,
            MediaElement media1, Button btnPlay, Button btnPause)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = "*.*",
                Filter = "Media Files (*.*)|*.*"
            };
            ofd.ShowDialog();
            try { media1.Source = new Uri(ofd.FileName); }
            catch { new NullReferenceException("Error"); }
            this.btnPlay_Click(sender, e, media1, btnPlay, btnPause);
        }

        public void slideMedia_MouseDown(object sender, RoutedEventArgs e,
            MediaElement media1)
        {
            _isDragging = true;
        }

        public void slideMedia_MouseUp(object sender, RoutedEventArgs e,
            MediaElement media1, Slider sliderMedia)
        {
            _isDragging = false;
            media1.Position = TimeSpan.FromSeconds(sliderMedia.Value);
        }

        public void media1_MediaOpened(object sender, RoutedEventArgs e,
            MediaElement media1, Slider sliderMedia)
        {
            if (media1.NaturalDuration.HasTimeSpan)
            {
                var ts = media1.NaturalDuration.TimeSpan;
                sliderMedia.Maximum = ts.TotalSeconds;
                sliderMedia.SmallChange = 1;
                sliderMedia.LargeChange = Math.Min(10, ts.Seconds/10);
            }
            this._timer.Start();
        }

        /*
         * Library buttons
         */
        public void btnMusic_Click(object sender, RoutedEventArgs e,
            ListView listLibraryMusic, ListView listLibraryVideo, ListView listLibraryImage)
        {
            if (listLibraryMusic.IsVisible)
                listLibraryMusic.Visibility = Visibility.Collapsed;
            else if (listLibraryMusic.Visibility == Visibility.Collapsed)
                listLibraryMusic.Visibility = Visibility.Visible;
            listLibraryVideo.Visibility = Visibility.Collapsed;
            listLibraryImage.Visibility = Visibility.Collapsed;
            this._library.RefreshMusic(listLibraryMusic);
        }

        public void btnVideo_Click(object sender, RoutedEventArgs e,
            ListView listLibraryMusic, ListView listLibraryVideo, ListView listLibraryImage)
        {
            if (listLibraryVideo.IsVisible)
                listLibraryVideo.Visibility = Visibility.Collapsed;
            else if (listLibraryVideo.Visibility == Visibility.Collapsed)
                listLibraryVideo.Visibility = Visibility.Visible;
            listLibraryMusic.Visibility = Visibility.Collapsed;
            listLibraryImage.Visibility = Visibility.Collapsed;
            this._library.RefreshVideo(listLibraryVideo);
        }

        public void btnImage_Click(object sender, RoutedEventArgs e,
            ListView listLibraryMusic, ListView listLibraryVideo, ListView listLibraryImage)
        {
            if (listLibraryImage.IsVisible)
                listLibraryImage.Visibility = Visibility.Collapsed;
            else if (listLibraryImage.Visibility == Visibility.Collapsed)
                listLibraryImage.Visibility = Visibility.Visible;
            listLibraryMusic.Visibility = Visibility.Collapsed;
            listLibraryVideo.Visibility = Visibility.Collapsed;
            this._library.RefreshImage(listLibraryImage);
        }

        /*
         * Library context menus
         */
        public void btnMusic_selectDirectory(object sender, RoutedEventArgs e,
            ListView listLibrary)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK) return;
            this._library.SetMusicDirectory(fbd.SelectedPath);
            this._library.RefreshMusic(listLibrary);
        }

        public void btnVideo_selectDirectory(object sender, RoutedEventArgs e,
            ListView listLibrary)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK) return;
            this._library.SetVideoDirectory(fbd.SelectedPath);
            this._library.RefreshVideo(listLibrary);
        }

        public void btnImage_selectDirectory(object sender, RoutedEventArgs e,
            ListView listLibrary)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK) return;
            this._library.SetImageDirectory(fbd.SelectedPath);
            this._library.RefreshImage(listLibrary);
        }

        /*
         * Library sort filter
         */
        public void listLibrary_Click(object sender, RoutedEventArgs e,
            ListView listLibrary)
        {
            this._library.SortColumn(e, listLibrary);
        }

        /*
         * Library load on double click
         */
        public void listLibraryMusicItem_DoubleClick(object sender, MouseButtonEventArgs e,
            MediaElement media1, Slider sliderMedia, ListView listLibrary,
            Button btnPlay, Button btnPause)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is ListViewItem))
                dep = VisualTreeHelper.GetParent(dep);
            if (dep == null)
                return;
            MyMusic item = (MyMusic)listLibrary.ItemContainerGenerator.ItemFromContainer(dep);
            try { media1.Source = new Uri(item.Path); }
            catch { new NullReferenceException("Error"); }
            listLibrary.Visibility = Visibility.Collapsed;
            this.btnPlay_Click(sender, e, media1, btnPlay, btnPause);
        }

        public void listLibraryVideoItem_DoubleClick(object sender, MouseButtonEventArgs e,
            MediaElement media1, Slider sliderMedia, ListView listLibrary,
            Button btnPlay, Button btnPause)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is ListViewItem))
                dep = VisualTreeHelper.GetParent(dep);
            if (dep == null)
                return;
            MyVideo item = (MyVideo)listLibrary.ItemContainerGenerator.ItemFromContainer(dep);
            try { media1.Source = new Uri(item.Path); }
            catch { new NullReferenceException("Error"); }
            listLibrary.Visibility = Visibility.Collapsed;
            this.btnPlay_Click(sender, e, media1, btnPlay, btnPause);
        }

        public void listLibraryImageItem_DoubleClick(object sender, MouseButtonEventArgs e,
            MediaElement media1, Slider sliderMedia, ListView listLibrary,
            Button btnPlay, Button btnPause)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is ListViewItem))
                dep = VisualTreeHelper.GetParent(dep);
            if (dep == null)
                return;
            MyImage item = (MyImage)listLibrary.ItemContainerGenerator.ItemFromContainer(dep);
            try { media1.Source = new Uri(item.Path); }
            catch { new NullReferenceException("Error"); }
            listLibrary.Visibility = Visibility.Collapsed;
            this.btnPlay_Click(sender, e, media1, btnPlay, btnPause);
        }

        /*
         * Playlist
         */
        public void menuPlaylistAdd_Click(object sender, RoutedEventArgs e,
            MediaElement media1, TreeView treePlaylist)
        {
            String playlistName = MyDialog.Prompt("Add current media to playlist", "Enter the playlist's name");
            if (String.IsNullOrEmpty(playlistName) || media1.Source == null ||
                String.IsNullOrEmpty(media1.Source.AbsolutePath)) return;
            this._playlist.AddElem(playlistName, media1.Source.AbsolutePath);
            this.RefreshPlaylists(treePlaylist);
        }

        public void RefreshPlaylists(TreeView treePlaylist)
        {
            this._playlist.RefreshPlaylists(treePlaylist);
        }

        public void treePlaylist_DoubleClick(object sender, MouseButtonEventArgs e,
            MediaElement media1, Slider sliderMedia, TreeView treePlaylist,
            Button btnPlay, Button btnPause)
        {
            MyMedia item = (MyMedia)treePlaylist.SelectedItem;
            try { media1.Source = new Uri(item.Path); }
            catch { new NullReferenceException("Error"); }
            this.btnPlay_Click(sender, e, media1, btnPlay, btnPause);
        }

        /*
         * Help
         */
        public void menuHelpHelp_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            MyDialog.Show("Help", "No help for now");
        }

        public void menuHelpAbout_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            MyDialog.Show("About us", "MyWindowsMediaPlayer by\n\ndovan_n - Noël DO VAN\ndurand_q - Jean-Noël DURAND\novoyan_s - Serguei OVOYAN\n peyrot_m - Matthieu PEYROT");
        }
    }
}
