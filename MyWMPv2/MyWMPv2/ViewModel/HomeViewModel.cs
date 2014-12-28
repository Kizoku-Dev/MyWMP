using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using MyWMPv2.Model;
using MyWMPv2.Utilities;
using MyWMPv2.View;

namespace MyWMPv2.ViewModel
{
    class HomeViewModel : ObservableObject
    {
        private Media _media;
        private PlaylistManager _playlistManager;
        private MusicViewModel _musicViewModel;
        private VideoViewModel _videoViewModel;
        private ImageViewModel _imageViewModel;
        private ICommand _mediaOpenCommand;
        private ICommand _mediaPlayCommand;
        private ICommand _mediaPauseCommand;
        private ICommand _mediaStopCommand;
        private bool _isDragging = false;
        private DispatcherTimer _timer;

        public HomeViewModel()
        {
            _media = new Media();
            _playlistManager = new PlaylistManager();
            _musicViewModel = new MusicViewModel();
            _videoViewModel = new VideoViewModel();
            _imageViewModel = new ImageViewModel();
            _musicViewModel.PropertyChanged += PropertyChangedHandler;
            _videoViewModel.PropertyChanged += PropertyChangedHandler;
            _imageViewModel.PropertyChanged += PropertyChangedHandler;
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "MusicDoubleClick":
                    Media.Source = new Uri(MusicViewModel.ItemSelected);
                    break;
                case "VideoDoubleClick":
                    Media.Source = new Uri(VideoViewModel.ItemSelected);
                    break;
                case "ImageDoubleClick":
                    Media.Source = new Uri(ImageViewModel.ItemSelected);
                    break;
                default:
                    OnPropertyChanged(e.PropertyName);
                    break;
            }
        }

        public Media Media
        {
            get { return _media; }
            set { _media = value; }
        }
        public PlaylistManager PlaylistManager
        {
            get { return _playlistManager; }
            set { _playlistManager = value; }
        }
        public MusicViewModel MusicViewModel
        {
            get { return _musicViewModel; }
            set { _musicViewModel = value; }
        }
        public VideoViewModel VideoViewModel
        {
            get { return _videoViewModel; }
            set { _videoViewModel = value; }
        }
        public ImageViewModel ImageViewModel
        {
            get { return _imageViewModel; }
            set { _imageViewModel = value; }
        }
        public ICommand MediaOpenCommand
        {
            get
            {
                if (_mediaOpenCommand == null)
                    _mediaOpenCommand = new DelegateCommand(MediaOpen);
                return _mediaOpenCommand;
            }
        }
        public ICommand MediaPlayCommand
        {
            get
            {
                if (_mediaPlayCommand == null)
                    _mediaPlayCommand = new DelegateCommand(MediaPlay);
                return _mediaPlayCommand;
            }
        }
        public ICommand MediaPauseCommand
        {
            get
            {
                if (_mediaPauseCommand == null)
                    _mediaPauseCommand = new DelegateCommand(MediaPause);
                return _mediaPauseCommand;
            }
        }
        public ICommand MediaStopCommand
        {
            get
            {
                if (_mediaStopCommand == null)
                    _mediaStopCommand = new DelegateCommand(MediaStop);
                return _mediaStopCommand;
            }
        }

        public void Init(MediaElement media, Slider sliderMedia)
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            _timer.Tick += new EventHandler((sender, e) => timer_Tick(media, sliderMedia));
        }

        private void timer_Tick(MediaElement media, Slider sliderMedia)
        {
            if (!_isDragging)
                sliderMedia.Value = media.Position.TotalSeconds;
        }
        private void MediaOpen(object sender)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = "*.*",
                Filter = "Media Files (*.*)|*.*"
            };
            ofd.ShowDialog();
            try
            {
                Media.Source = new Uri(ofd.FileName);
                Media.State = MediaState.Play;
                Console.WriteLine("Media opened and played");
            }
            catch (Exception e) { Console.WriteLine("Error media opening"); }
        }

        public void CallMediaPlay()
        {
            MediaPlay(null);
        }
        private void MediaPlay(object sender)
        {
            Media.State = MediaState.Play;
        }
        private void MediaPause(object sender)
        {
            Media.State = MediaState.Pause;
        }
        private void MediaStop(object sender)
        {
            Media.State = MediaState.Stop;
        }
        /*
         * Event Handler
         */
        public void SliderMedia_MouseDown(object sender, RoutedEventArgs e)
        {
            _isDragging = true;
        }
        
        public void SliderMedia_MouseUp(object sender, RoutedEventArgs e,
            MediaElement media, Slider sliderMedia)
        {
            _isDragging = false;
            media.Position = TimeSpan.FromSeconds(sliderMedia.Value);
        }

        public void Media_MediaOpened(object sender, RoutedEventArgs routedEventArgs,
            MediaElement media, Slider sliderMedia)
        {
            if (media.NaturalDuration.HasTimeSpan)
            {
                var ts = media.NaturalDuration.TimeSpan;
                sliderMedia.Maximum = ts.TotalSeconds;
                sliderMedia.SmallChange = 1;
                sliderMedia.LargeChange = Math.Min(10, ts.Seconds / 10);
            }
            _timer.Start();
        }

        /*
         * Playlist
         */
        public void Playlist_Click(object sender, RoutedEventArgs routedEventArg,
            TreeView treePlaylists)
        {
            if (treePlaylists.Visibility == Visibility.Visible)
                treePlaylists.Visibility = Visibility.Collapsed;
            else if (treePlaylists.Visibility == Visibility.Collapsed)
                treePlaylists.Visibility = Visibility.Visible;
            _playlistManager.RefreshPlaylists(treePlaylists);
        }

        public void Playlist_Add(object sender, RoutedEventArgs routedEventArgs,
            MediaElement media, TreeView treePlaylist)
        {
            String playlistName = MyDialog.Prompt("Add current media to playlist", "Enter the playlist's name");
            if (String.IsNullOrEmpty(playlistName) || media.Source == null ||
                String.IsNullOrEmpty(media.Source.AbsolutePath)) return;
            _playlistManager.AddElem(playlistName, media.Source.AbsolutePath);
            RefreshPlaylists(treePlaylist);
        }

        public void RefreshPlaylists(TreeView treePlaylist)
        {
            _playlistManager.RefreshPlaylists(treePlaylist);
        }
        public void TreePlaylist_DoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs,
            MediaElement media, TreeView treePlaylist)
        {
            if (treePlaylist.SelectedItem.GetType() != typeof (MyMedia))
                return;
            MyMedia item = (MyMedia)treePlaylist.SelectedItem;
            try { Media.Source = new Uri(item.Path); }
            catch (Exception e) { Console.WriteLine("Error:"+e); }
            OnPropertyChanged("Source");
        }
    }
}
