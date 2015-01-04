using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MyWMPv2.ViewModel;

namespace MyWMPv2.View
{
    /// <summary>
    /// Logique d'interaction pour ApplicationView.xaml
    /// </summary>
    public partial class ApplicationView : Window
    {
        #region Private member variables
        private readonly ApplicationViewModel _applicationViewModel;
        private bool _full = false;
        #endregion Private member variables

        public ApplicationView()
        {
            InitializeComponent();
            _applicationViewModel = new ApplicationViewModel(ListMusic, ListVideo, ListImage);
            DataContext = _applicationViewModel;
            _applicationViewModel.HomeViewModel.Init(Media, SliderMedia);
        }

        #region Event Handler
        /*
         * ****** Event Handler
         */
        /*
         * Window
         */
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void TopClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (_full)
                    WindowState = WindowState.Normal;
                else
                    WindowState = WindowState.Maximized;
                _full = !_full;
            }
        }
        private void Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            NotifIcon.Visibility = Visibility.Collapsed;
            NotifIcon.Icon = null;
            Environment.Exit(0);
        }

        /*
         * Menu
         */
        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.ChangeTheme_Click(ListMusic, ListVideo, ListImage, TreePlaylist);
        }

        /*
         * Media
         */
        private void Media_MediaOpened(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.Media_MediaOpened(sender, e, Media, SliderMedia);
        }
        private void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.Media_MediaEnded();
        }
        private void Buffering_Started(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.Buffering_Started(sender, e, Media);
        }
        private void Buffering_Ended(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.Buffering_Ended(sender, e, Media);
        }
        private void Media_Drop(object sender, DragEventArgs e)
        {
            _applicationViewModel.HomeViewModel.Media_Drop(sender, e);
        }
        private void Media_ChangeOpacity(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.Media_ChangeOpacity(sender, e);
        }
        private void Media_ChangeSpeedRatio(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.Media_ChangeSpeedRatio(sender, e, Media);
        }

        /*
         * Slider
         */
        private void SliderMedia_MouseDown(object sender, DragStartedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.SliderMedia_MouseDown(sender, e);
        }
        private void SliderMedia_MouseUp(object sender, DragCompletedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.SliderMedia_MouseUp(sender, e, Media, SliderMedia);
        }
        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Media.Volume = (double)SliderVolume.Value;
        }

        /*
         * Music
         */
        private void Music_Click(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.MusicViewModel.Music_Click(sender, e, ListMusic, ListVideo, ListImage, _applicationViewModel.TE.FgList);
        }
        private void Music_Directory(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.MusicViewModel.Music_Directory(sender, e, ListMusic, _applicationViewModel.TE.FgList);
        }
        private void Music_SearchFilename(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.MusicViewModel.Music_SearchFilename(_applicationViewModel.TE.FgList);
        }
        private void Music_SearchAuthor(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.MusicViewModel.Music_SearchAuthor(_applicationViewModel.TE.FgList);
        }
        private void Music_SearchAlbum(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.MusicViewModel.Music_SearchAlbum(_applicationViewModel.TE.FgList);
        }
        private void ListMusic_Click(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.MusicViewModel.ListMusic_Click(sender, e, _applicationViewModel.TE.CurrentTheme);
        }
        private void ListMusic_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            _applicationViewModel.HomeViewModel.MusicViewModel.ListMusic_DoubleClick(sender, e, ListMusic);
        }

        /*
         * Video
         */
        private void Video_Click(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.VideoViewModel.Video_Click(sender, e, ListMusic, ListVideo, ListImage, _applicationViewModel.TE.FgList);
        }
        private void Video_Directory(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.VideoViewModel.Video_Directory(sender, e, ListVideo, _applicationViewModel.TE.FgList);
        }
        private void Video_Search(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.VideoViewModel.Video_Search(_applicationViewModel.TE.FgList);
        }
        private void ListVideo_Click(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.VideoViewModel.ListVideo_Click(sender, e, _applicationViewModel.TE.CurrentTheme);
        }
        private void ListVideo_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            _applicationViewModel.HomeViewModel.VideoViewModel.ListVideo_DoubleClick(sender, e, ListVideo);
        }

        /*
         * Image
         */
        private void Image_Click(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.ImageViewModel.Image_Click(sender, e, ListMusic, ListVideo, ListImage, _applicationViewModel.TE.FgList);
        }
        private void Image_Directory(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.ImageViewModel.Image_Directory(sender, e, ListImage, _applicationViewModel.TE.FgList);
        }
        private void Image_Search(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.ImageViewModel.Image_Search(_applicationViewModel.TE.FgList);
        }
        private void ListImage_Click(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.ImageViewModel.ListImage_Click(sender, e, _applicationViewModel.TE.CurrentTheme);
        }
        private void ListImage_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            _applicationViewModel.HomeViewModel.ImageViewModel.ListImage_DoubleClick(sender, e, ListImage);
        }

        /*
         * Playlist
         */
        private void Playlist_Click(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.Playlist_Click(sender, e, TreePlaylist, _applicationViewModel.TE.FgList);
        }
        private void Playlist_Add(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.Playlist_Add(sender, e, Media, TreePlaylist, _applicationViewModel.TE.FgList);
        }
        private void ListMusic_AddPlaylist(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.ListView_AddPlaylist(sender, e, Media, TreePlaylist, ListMusic, _applicationViewModel.TE.FgList);
        }
        private void ListVideo_AddPlaylist(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.ListView_AddPlaylist(sender, e, Media, TreePlaylist, ListVideo, _applicationViewModel.TE.FgList);
        }
        private void ListImage_AddPlaylist(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.ListView_AddPlaylist(sender, e, Media, TreePlaylist, ListImage, _applicationViewModel.TE.FgList);
        }
        private void ListImage_Diaporama(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.ListImage_Diaporama(sender, e, Media, TreePlaylist, ListImage, _applicationViewModel.TE.FgList);
        }
        private void TreePlaylist_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            _applicationViewModel.HomeViewModel.TreePlaylist_DoubleClick(sender, e, TreePlaylist);
        }
        private void Playlist_Start(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.Playlist_Start(sender, e, TreePlaylist);
        }
        private void Playlist_StartRandom(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.Playlist_StartRandom(sender, e, TreePlaylist);
        }
        private void Playlist_Rename(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.Playlist_Rename(sender, e, TreePlaylist, _applicationViewModel.TE.FgList);
        }
        private void Playlist_Delete(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.Playlist_Delete(sender, e, TreePlaylist, _applicationViewModel.TE.FgList);
        }

        /*
         * Youtube
         */
        private void Open_Youtube(object sender, RoutedEventArgs e)
        {
            _applicationViewModel.HomeViewModel.Open_Youtube(sender, e, Media);
        }
        #endregion Event Handler
    }
}
