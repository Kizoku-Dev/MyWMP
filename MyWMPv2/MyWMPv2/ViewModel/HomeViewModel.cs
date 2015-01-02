﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using MyToolkit.Multimedia;
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
        private DispatcherTimer _timer;
        private DispatcherTimer _timerProgress;
        private bool _isDragging = false;
        private bool _isPlaylist = false;
        private int _indexPlaylist;

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
                    MediaPlayNormal();
                    break;
                case "VideoDoubleClick":
                    Media.Source = new Uri(VideoViewModel.ItemSelected);
                    MediaPlayNormal();
                    break;
                case "ImageDoubleClick":
                    Media.Source = new Uri(ImageViewModel.ItemSelected);
                    MediaPlayNormal();
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
            _timerProgress = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            _timerProgress.Tick += new EventHandler((sender, e) => timerProgress_Tick(media));
        }

        private void timer_Tick(MediaElement media, Slider sliderMedia)
        {
            if (!_isDragging)
            {
                sliderMedia.Value = media.Position.TotalSeconds;
                if (media.NaturalDuration.HasTimeSpan)
                {
                    TimeSpan ts = media.Position;
                    Media.Position = string.Format("{0:00}:{1:00}", (ts.Hours * 60) + ts.Minutes, ts.Seconds);
                    media.Position.TotalSeconds.ToString();
                }
            }
        }
        private void MediaOpen(object sender)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    AddExtension = true,
                    DefaultExt = "*.mp3",
                    Filter = "Media files (*.mp3,*.wav,*.ogg,*.flac,*.avi,*.mp4,*.mkv,*.jpg,*.jpeg,*.png,*.gif,*.bmp)|*.mp3;*.wav;*.ogg;*.flac;*.avi;*.mp4;*.mkv;*.jpg;*.jpeg;*.png;*.gif;*.bmp"
                };
                ofd.ShowDialog();
                if (ofd.FileName.Equals("")) return;
                Media.Source = new Uri(ofd.FileName);
                MediaPlayNormal();
                Console.WriteLine("Media open success : " + ofd.FileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error media open");
                MessageBox.Show(e.Message, "Error media open", MessageBoxButton.OK);
            }
        }
        private void MediaPlay(object sender)
        {
            Media.State = MediaState.Play;
        }
        private void MediaPlayNormal()
        {
            Media.State = MediaState.Play;
            _isPlaylist = false;
        }
        private void MediaPause(object sender)
        {
            Media.State = MediaState.Pause;
        }
        private void MediaStop(object sender)
        {
            Media.State = MediaState.Stop;
        }
        private void timerProgress_Tick(MediaElement media)
        {
            Media.Progress = (media.BufferingProgress * 100).ToString();
            Console.WriteLine("Buffering progress:" + Media.Progress);
        }
        /*
         * ****** Event Handler
         */

        /*
         * Media
         */
        public void Media_MediaOpened(object sender, RoutedEventArgs routedEventArgs,
            MediaElement media, Slider sliderMedia)
        {
            if (media.NaturalDuration.HasTimeSpan)
            {
                var ts = media.NaturalDuration.TimeSpan;
                Media.PositionMax = string.Format("{0:00}:{1:00}", (ts.Hours * 60) + ts.Minutes, ts.Seconds);
                sliderMedia.Maximum = ts.TotalSeconds;
                sliderMedia.SmallChange = 1;
                sliderMedia.LargeChange = Math.Min(10, ts.Seconds / 10);
            }
            _timer.Start();
        }
        public void Media_MediaEnded(object sender, RoutedEventArgs routedEventArgs, MediaElement media, Slider sliderMedia)
        {
            if (_isPlaylist)
            {
                ++_indexPlaylist;
                if (_indexPlaylist >= _playlistManager.CurrentPlaylist.Count)
                {
                    _isPlaylist = false;
                    Console.WriteLine("Playlist successfully finished !");
                }
                else
                {
                    Console.WriteLine("Playing : " + _playlistManager.CurrentPlaylist[_indexPlaylist]);
                    Media.Source = new Uri(_playlistManager.CurrentPlaylist[_indexPlaylist]);
                    MediaPlay(null);
                }
            }
        }
        public void Buffering_Started(object sender, RoutedEventArgs routedEventArgs, MediaElement media)
        {
            Console.WriteLine("Buffering started");
            _timerProgress.Start();
        }
        public void Buffering_Ended(object sender, RoutedEventArgs routedEventArgs, MediaElement media)
        {
            Console.WriteLine("Buffering stopped");
            _timerProgress.Stop();
            Media.Progress = "0";
        }

        /*
         * Slider
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
            String playlistName = MyDialog.Prompt("Add current media to playlist", "Enter the playlist's name", MyDialog.Size.Normal);
            if (String.IsNullOrEmpty(playlistName) || media.Source == null ||
                String.IsNullOrEmpty(media.Source.AbsolutePath)) return;
            _playlistManager.AddElem(playlistName, media.Source.AbsolutePath);
            _playlistManager.RefreshPlaylists(treePlaylist);
        }
        public void TreePlaylist_DoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs,
            TreeView treePlaylist)
        {
            if (treePlaylist.SelectedItem.GetType() != typeof (MyMedia))
                return;
            MyMedia item = (MyMedia)treePlaylist.SelectedItem;
            try
            {
                Console.WriteLine("A : " + item.Path);
                Media.Source = new Uri(item.Path);
                MediaPlayNormal();
            }
            catch (Exception e) { Console.WriteLine("Error:"+e); }
        }
        public void Playlist_Start(object sender, RoutedEventArgs e,
            TreeView treePlaylist)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                ContextMenu cm = mi.CommandParameter as ContextMenu;
                if (cm != null)
                {
                    TextBlock item = cm.PlacementTarget as TextBlock;
                    if (item != null)
                    {
                        _isPlaylist = true;
                        _indexPlaylist = 0;
                        _playlistManager.SetCurrentPlaylist(item.Text);
                        try
                        {
                            Console.WriteLine("Playing : " + _playlistManager.CurrentPlaylist[_indexPlaylist]);
                            Media.Source = new Uri(_playlistManager.CurrentPlaylist[_indexPlaylist]);
                            MediaPlay(null);
                        }
                        catch (Exception err) { Console.WriteLine("Error:" + err); }
                    }
                }
            }
        }

        /*
         * Youtube
         */
        public async void Open_Youtube(object sender, RoutedEventArgs routedEventArgs, MediaElement media)
        {
            String videoId = MyDialog.Prompt("Enter the youtube video id",
                "for https://www.youtube.com/watch?v=DfKcEehLwm0\n-> Enter \"DfKcEehLwm0\"", MyDialog.Size.Big);
            try
            {
                var url = await YouTube.GetVideoUriAsync(videoId, YouTubeQuality.Unknown);
                if (url != null && url.Uri != null && url.HasVideo)
                {
                    Media.Source = url.Uri;
                    MediaPlayNormal();
                    Console.WriteLine("Open from youtube OK");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error youtube api");
                MessageBox.Show(e.Message, "Error youtube api", MessageBoxButton.OK);
            }
        }
    }
}
