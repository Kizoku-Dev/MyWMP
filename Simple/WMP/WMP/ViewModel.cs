using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ListView = System.Windows.Controls.ListView;
using ListViewItem = System.Windows.Controls.ListViewItem;

namespace WMP
{
    class ViewModel
    {
        private readonly Library _library;
        private readonly Playlist _playlist;
        private bool _slideChanged;

        public ViewModel()
        {
            this._library = new Library();   
            this._playlist = new Playlist();
        }

        /*
         * Controller buttons
         */
        public void btnPlay_Click(object sender, RoutedEventArgs e,
            MediaElement media1, Slider sliderMedia)
        {
            media1.Play();
            MediaClock clock = media1.Clock;
            if (clock != null)
            {
                if (clock.IsPaused)
                    clock.Controller.Resume();
                else
                    clock.Controller.Pause();
            }
            else
            {
                MediaTimeline timeline = new MediaTimeline(media1.Source);
                clock = timeline.CreateClock();
                clock.CurrentTimeInvalidated += new EventHandler((sender1, e1) => Clock_CurrentTimeInvalidated(sender, e, media1, sliderMedia));
                media1.Clock = clock;
            }
        }

        public void Clock_CurrentTimeInvalidated(object sender, EventArgs e,
            MediaElement media1, Slider sliderMedia)
        {
            if (media1.Clock == null || this._slideChanged)
                return;
            if (media1.Clock.CurrentTime != null)
                sliderMedia.Value = media1.Clock.CurrentTime.Value.TotalMilliseconds;
        }

        public void btnStop_Click(object sender, RoutedEventArgs e,
            MediaElement media1)
        {
            media1.Stop();
            media1.Clock = null;
        }

        public void menuFileOpen_Click(object sender, RoutedEventArgs e,
            MediaElement media1, Slider sliderMedia)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media Files (*.*)|*.*";
            ofd.ShowDialog();

            try { media1.Source = new Uri(ofd.FileName); }
            catch { new NullReferenceException("Error"); }

            this.btnStop_Click(sender, e, media1);
            this.btnPlay_Click(sender, e, media1, sliderMedia);
        }

        public void slideMedia_MouseDown(object sender, RoutedEventArgs e,
            MediaElement media1)
        {
            this._slideChanged = true;
        }

        public void slideMedia_MouseUp(object sender, RoutedEventArgs e,
            MediaElement media1, Slider sliderMedia)
        {
            MediaClock clock = media1.Clock;
            if (clock != null)
            {
                TimeSpan mOffset = TimeSpan.FromMilliseconds(sliderMedia.Value);
                if (clock.Controller != null) clock.Controller.Seek(mOffset, TimeSeekOrigin.BeginTime);
            }
            this._slideChanged = false;
        }

        public void media1_MediaOpened(object sender, RoutedEventArgs e,
            MediaElement media1, Slider sliderMedia)
        {
            if (media1.NaturalDuration.HasTimeSpan)
                sliderMedia.Maximum = media1.NaturalDuration.TimeSpan.TotalMilliseconds;
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
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this._library.SetMusicDirectory(fbd.SelectedPath);
                this._library.RefreshMusic(listLibrary);
            }
        }

        public void btnVideo_selectDirectory(object sender, RoutedEventArgs e,
            ListView listLibrary)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this._library.SetVideoDirectory(fbd.SelectedPath);
                this._library.RefreshVideo(listLibrary);
            }
        }

        public void btnImage_selectDirectory(object sender, RoutedEventArgs e,
            ListView listLibrary)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this._library.SetImageDirectory(fbd.SelectedPath);
                this._library.RefreshImage(listLibrary);
            }
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
            MediaElement media1, Slider sliderMedia, ListView listLibrary)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is ListViewItem))
                dep = VisualTreeHelper.GetParent(dep);
            if (dep == null)
                return;
            MyMusic item = (MyMusic)listLibrary.ItemContainerGenerator.ItemFromContainer(dep);

            try { media1.Source = new Uri(item.Path); }
            catch { new NullReferenceException("Error"); }

            listLibrary.Visibility = Visibility.Hidden;
            this.btnStop_Click(sender, e, media1);
            this.btnPlay_Click(sender, e, media1, sliderMedia);
        }

        public void listLibraryVideoItem_DoubleClick(object sender, MouseButtonEventArgs e,
            MediaElement media1, Slider sliderMedia, ListView listLibrary)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is ListViewItem))
                dep = VisualTreeHelper.GetParent(dep);
            if (dep == null)
                return;
            MyVideo item = (MyVideo)listLibrary.ItemContainerGenerator.ItemFromContainer(dep);

            try { media1.Source = new Uri(item.Path); }
            catch { new NullReferenceException("Error"); }

            listLibrary.Visibility = Visibility.Hidden;
            this.btnStop_Click(sender, e, media1);
            this.btnPlay_Click(sender, e, media1, sliderMedia);
        }

        public void listLibraryImageItem_DoubleClick(object sender, MouseButtonEventArgs e,
            MediaElement media1, Slider sliderMedia, ListView listLibrary)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is ListViewItem))
                dep = VisualTreeHelper.GetParent(dep);
            if (dep == null)
                return;
            MyImage item = (MyImage)listLibrary.ItemContainerGenerator.ItemFromContainer(dep);

            try { media1.Source = new Uri(item.Path); }
            catch { new NullReferenceException("Error"); }

            listLibrary.Visibility = Visibility.Hidden;
            this.btnStop_Click(sender, e, media1);
            this.btnPlay_Click(sender, e, media1, sliderMedia);
        }

        /*
         * Playlist add elem (default : MyDocuments/MyWMP/Playlist.txt)
         */
        public void menuPlaylistAdd_Click(object sender, RoutedEventArgs e,
            MediaElement media1)
        {
            String playlistName = MyDialog.Prompt("Add current media to playlist", "Enter the playlist's name");
            if (!String.IsNullOrEmpty(playlistName) && !String.IsNullOrEmpty(media1.Source.AbsolutePath))
            {
                this._playlist.AddElem(playlistName, media1.Source.AbsolutePath);
            }
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
