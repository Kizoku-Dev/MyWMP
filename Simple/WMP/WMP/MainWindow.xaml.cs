using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace WMP
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Init View
            media1.Volume = (double) sliderVolume.Value;
            // Init ViewModel
            this._viewModel = new ViewModel(sliderMedia, media1);
            this._viewModel.RefreshPlaylists(treePlaylist);
        }

        /*
         * Button event
         */
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (media1.Source != null)
                this._viewModel.btnPlay_Click(sender, e, media1, btnPlay, btnPause);
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (media1.Source != null)
                this._viewModel.btnPause_Click(sender, e, media1, btnPlay, btnPause);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (media1.Source != null)
                this._viewModel.btnStop_Click(sender, e, media1, btnPlay, btnPause);
        }

        private void menuFileOpen_Click(object sender, RoutedEventArgs e)
        {
            this._viewModel.menuFileOpen_Click(sender, e, media1, btnPlay, btnPause);
        }
        /*
         * Slide media event
         */
        private void slideMedia_MouseDown(object sender, RoutedEventArgs e)
        {
            this._viewModel.slideMedia_MouseDown(sender, e, media1);
        }

        private void slideMedia_MouseUp(object sender, RoutedEventArgs e)
        {
            this._viewModel.slideMedia_MouseUp(sender, e, media1, sliderMedia);
        }

        /*
         * Slide volume event
         */
        private void slideVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media1.Volume = (double)sliderVolume.Value;
        }

        /*
         * Media opened event
         */
        private void media1_MediaOpened(object sender, RoutedEventArgs e)
        {
            this._viewModel.media1_MediaOpened(sender, e, media1, sliderMedia);
        }

        /*
         * Library button event
         */
        private void btnMusic_Click(object sender, RoutedEventArgs e)
        {
            this._viewModel.btnMusic_Click(sender, e, listLibraryMusic, listLibraryVideo, listLibraryImage);
        }

        private void btnVideo_Click(object sender, RoutedEventArgs e)
        {
            this._viewModel.btnVideo_Click(sender, e, listLibraryMusic, listLibraryVideo, listLibraryImage);
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            this._viewModel.btnImage_Click(sender, e, listLibraryMusic, listLibraryVideo, listLibraryImage);
        }

        /*
         * Library context menu event
         */
        private void btnMusic_selectDirectory(object sender, RoutedEventArgs e)
        {
            this._viewModel.btnMusic_selectDirectory(sender, e, listLibraryMusic);
        }

        private void btnVideo_selectDirectory(object sender, RoutedEventArgs e)
        {
            this._viewModel.btnVideo_selectDirectory(sender, e, listLibraryVideo);
        }

        private void btnImage_selectDirectory(object sender, RoutedEventArgs e)
        {
            this._viewModel.btnImage_selectDirectory(sender, e, listLibraryImage);
        }

        /*
         * Library sort row event
         */
        private void listLibraryMusic_Click(object sender, RoutedEventArgs e)
        {
            this._viewModel.listLibrary_Click(sender, e, listLibraryMusic);
        }

        private void listLibraryVideo_Click(object sender, RoutedEventArgs e)
        {
            this._viewModel.listLibrary_Click(sender, e, listLibraryVideo);
        }

        private void listLibraryImage_Click(object sender, RoutedEventArgs e)
        {
            this._viewModel.listLibrary_Click(sender, e, listLibraryImage);
        }

        /*
         * Library play media on double click event
         */
        private void listLibraryMusicItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            this._viewModel.listLibraryMusicItem_DoubleClick(sender, e, media1, sliderMedia, listLibraryMusic, btnPlay, btnPause);
        }

        private void listLibraryVideoItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            this._viewModel.listLibraryVideoItem_DoubleClick(sender, e, media1, sliderMedia, listLibraryVideo, btnPlay, btnPause);
        }

        private void listLibraryImageItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            this._viewModel.listLibraryImageItem_DoubleClick(sender, e, media1, sliderMedia, listLibraryImage, btnPlay, btnPause);
        }

        /*
         * Playlist add elem (default : MyDocuments/MyWMP/Playlist.txt)
         */
        private void menuPlaylistAdd_Click(object sender, RoutedEventArgs e)
        {
            this._viewModel.menuPlaylistAdd_Click(sender, e, media1, treePlaylist);
        }

        private void treePlaylist_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            this._viewModel.treePlaylist_DoubleClick(sender, e, media1, sliderMedia, treePlaylist, btnPlay, btnPause);
        }

        /*
         * Help
         */
        private void menuHelpAbout_Click(object sender, RoutedEventArgs e)
        {
            this._viewModel.menuHelpAbout_Click(sender, e);
        }

        private void menuHelpHelp_Click(object sender, RoutedEventArgs e)
        {
            this._viewModel.menuHelpHelp_Click(sender, e);
        }
    }
}
