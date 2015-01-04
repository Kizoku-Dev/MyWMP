using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
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
        #region Private member variables
        private Media _media;
        private PlaylistManager _playlistManager;
        private MusicViewModel _musicViewModel;
        private VideoViewModel _videoViewModel;
        private ImageViewModel _imageViewModel;
        private ICommand _mediaOpenCommand;
        private ICommand _mediaPlayCommand;
        private ICommand _mediaPauseCommand;
        private ICommand _mediaStopCommand;
        private ICommand _mediaPrevCommand;
        private ICommand _mediaNextCommand;
        private ICommand _mediaRepeatCommand;
        private ICommand _mediaPlayPauseCommand;
        private Visibility _playVisibility;
        private Visibility _pauseVisibility;
        private Visibility _playlistVisibility;
        private Visibility _repeatVisibility;
        private Visibility _noRepeatVisibility;
        private DispatcherTimer _timer;
        private DispatcherTimer _timerProgress;
        private bool _isDragging = false;
        private bool _isRepeat = false;
        private bool _isPlaylist = false;
        private int _indexPlaylist;
        #endregion Private member variables

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
            _media.Opacity = 1;
            _playVisibility = Visibility.Visible;
            _pauseVisibility = Visibility.Collapsed;
            _playlistVisibility = Visibility.Collapsed;
            _repeatVisibility = Visibility.Collapsed;
            _noRepeatVisibility = Visibility.Visible;
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

        public void Init(MediaElement media, Slider sliderMedia)
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            _timer.Tick += new EventHandler((sender, e) => timer_Tick(media, sliderMedia));
            _timerProgress = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            _timerProgress.Tick += new EventHandler((sender, e) => timerProgress_Tick(media));
        }

        #region Public member variables
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
        public ICommand MediaPrevCommand
        {
            get
            {
                if (_mediaPrevCommand == null)
                    _mediaPrevCommand = new DelegateCommand(MediaPrev);
                return _mediaPrevCommand;
            }
        }
        public ICommand MediaNextCommand
        {
            get
            {
                if (_mediaNextCommand == null)
                    _mediaNextCommand = new DelegateCommand(MediaNext);
                return _mediaNextCommand;
            }
        }
        public ICommand MediaRepeatCommand
        {
            get
            {
                if (_mediaRepeatCommand == null)
                    _mediaRepeatCommand = new DelegateCommand(MediaRepeat);
                return _mediaRepeatCommand;
            }
        }
        public ICommand MediaPlayPauseCommand
        {
            get
            {
                if (PlayVisibility == Visibility.Visible &&
                    PauseVisibility == Visibility.Collapsed)
                    _mediaPlayPauseCommand = _mediaPlayCommand;
                else if (PauseVisibility == Visibility.Visible &&
                    PlayVisibility == Visibility.Collapsed)
                    _mediaPlayPauseCommand = _mediaPauseCommand;
                return _mediaPlayPauseCommand;
            }
        }
        public Visibility PlayVisibility
        {
            get { return _playVisibility; }
            set
            {
                _playVisibility = value;
                OnPropertyChanged("PlayVisibility");
                OnPropertyChanged("MediaPlayPauseCommand");
            }
        }
        public Visibility PauseVisibility
        {
            get { return _pauseVisibility; }
            set
            {
                _pauseVisibility = value;
                OnPropertyChanged("PauseVisibility");
                OnPropertyChanged("MediaPlayPauseCommand");
            }
        }
        public Visibility PlaylistVisibility
        {
            get { return _playlistVisibility; }
            set
            {
                _playlistVisibility = value;
                OnPropertyChanged("PlaylistVisibility");
            }
        }
        public Visibility RepeatVisibility
        {
            get { return _repeatVisibility; }
            set
            {
                _repeatVisibility = value;
                OnPropertyChanged("RepeatVisibility");
            }
        }
        public Visibility NoRepeatVisibility
        {
            get { return _noRepeatVisibility; }
            set
            {
                _noRepeatVisibility = value;
                OnPropertyChanged("NoRepeatVisibility");
            }
        }
        #endregion Public member variables

        #region Private member methods
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
            PlayVisibility = Visibility.Collapsed;
            PauseVisibility = Visibility.Visible;
        }
        private void MediaPlayNormal()
        {
            Media.State = MediaState.Play;
            _isPlaylist = false;
            PlayVisibility = Visibility.Collapsed;
            PauseVisibility = Visibility.Visible;
            PlaylistVisibility = Visibility.Collapsed;
        }
        private void MediaPause(object sender)
        {
            Media.State = MediaState.Pause;
            PlayVisibility = Visibility.Visible;
            PauseVisibility = Visibility.Collapsed;
        }
        private void MediaStop(object sender)
        {
            Media.State = MediaState.Stop;
            PlayVisibility = Visibility.Visible;
            PauseVisibility = Visibility.Collapsed;
        }
        private void MediaPrev(object sender)
        {
            if (_isPlaylist && _indexPlaylist != 0)
            {
                --_indexPlaylist;
                Console.WriteLine("Playing : " + _playlistManager.CurrentPlaylist[_indexPlaylist]);
                Media.Source = new Uri(_playlistManager.CurrentPlaylist[_indexPlaylist]);
                MediaPlay(null);
            }
        }
        private void MediaNext(object sender)
        {
            if (_isPlaylist)
            {
                Media.State = MediaState.Close;
                Media_MediaEnded();
            }
        }
        private void MediaRepeat(object sender)
        {
            if (_isRepeat)
            {
                RepeatVisibility = Visibility.Collapsed;
                NoRepeatVisibility = Visibility.Visible;
            }
            else
            {
                RepeatVisibility = Visibility.Visible;
                NoRepeatVisibility = Visibility.Collapsed;
            }
            _isRepeat = !_isRepeat;
        }
        private void timerProgress_Tick(MediaElement media)
        {
            Media.Progress = (media.BufferingProgress * 100).ToString();
            Console.WriteLine("Buffering progress:" + Media.Progress);
        }
        #endregion Private member methods

        #region Event Handler
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
        public void Media_MediaEnded()
        {
            if (_isPlaylist)
            {
                ++_indexPlaylist;
                if (_indexPlaylist >= _playlistManager.CurrentPlaylist.Count)
                {
                    _isPlaylist = false;
                    PlaylistVisibility = Visibility.Collapsed;
                    Console.WriteLine("Playlist successfully finished !");
                }
                else
                {
                    Console.WriteLine("Playing : " + _playlistManager.CurrentPlaylist[_indexPlaylist]);
                    Media.Source = new Uri(_playlistManager.CurrentPlaylist[_indexPlaylist]);
                    MediaPlay(null);
                }
            }
            else if (_isRepeat)
            {
                MediaStop(null);
                MediaPlay(null);
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
        public void Media_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    String[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
                    Media.Source = new Uri(files[0]);
                    MediaPlayNormal();
                    Console.WriteLine("Media drag open success : " + files[0]);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("Error drag");
                MessageBox.Show(err.Message, "Error drag", MessageBoxButton.OK);
            }
        }
        public void Media_ChangeOpacity(object sender, RoutedEventArgs routedEventArgs)
        {
            String value = MyDialog.Prompt("Change media settings", "Enter opacity desired (0-1) :", MyDialog.Size.Big).Replace(".", ",");
            if (String.IsNullOrEmpty(value)) return;
            Double tmp = 1;
            try
            {
                tmp = Convert.ToDouble(value);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            Media.Opacity = tmp;
        }
        public void Media_ChangeSpeedRatio(object sender, RoutedEventArgs routedEventArgs, MediaElement media)
        {
            String value = MyDialog.Prompt("Change media settings", "Enter speed ratio desired (0-1) :", MyDialog.Size.Big).Replace(".", ",");
            if (String.IsNullOrEmpty(value)) return;
            Double tmp = 1;
            try
            {
                tmp = Convert.ToDouble(value);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            media.SpeedRatio = tmp;
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
            TreeView treePlaylists, String fgList)
        {
            if (treePlaylists.Visibility == Visibility.Visible)
                treePlaylists.Visibility = Visibility.Collapsed;
            else if (treePlaylists.Visibility == Visibility.Collapsed)
                treePlaylists.Visibility = Visibility.Visible;
            _playlistManager.RefreshPlaylists(treePlaylists, fgList);
        }
        public void Playlist_Add(object sender, RoutedEventArgs routedEventArgs,
            MediaElement media, TreeView treePlaylist, String fgList)
        {
            String playlistName = MyDialog.Prompt("Add current media to playlist", "Enter the playlist's name", MyDialog.Size.Normal);
            if (String.IsNullOrEmpty(playlistName) || media.Source == null ||
                String.IsNullOrEmpty(media.Source.AbsolutePath)) return;
            _playlistManager.AddElem(playlistName, media.Source.AbsolutePath);
            _playlistManager.RefreshPlaylists(treePlaylist, fgList);
        }
        public void ListView_AddPlaylist(object sender, RoutedEventArgs routedEventArgs,
            MediaElement media, TreeView treePlaylist, ListView list, String fgList)
        {
            IMyMedia item = list.SelectedItem as IMyMedia;
            if (item == null)
                return;
            String playlistName = MyDialog.Prompt("Add current media to playlist", "Enter the playlist's name", MyDialog.Size.Normal);
            if (String.IsNullOrEmpty(playlistName))
            {
                MessageBox.Show("Incorrect name", "Error adding to playlist", MessageBoxButton.OK);
                return;
            }
            _playlistManager.AddElem(playlistName, item.Path);
            _playlistManager.RefreshPlaylists(treePlaylist, fgList);
        }
        public void ListImage_Diaporama(object sender, RoutedEventArgs routedEventArgs,
            MediaElement media, TreeView treePlaylist, ListView list, String fgList)
        {
            String name = MyDialog.Prompt("Create a diaporama", "Enter the diaporama's name", MyDialog.Size.Normal);
            if (String.IsNullOrEmpty(name))
            {
                MessageBox.Show("Incorrect name", "Error creating diaporama", MessageBoxButton.OK);
                return;
            }
            IList selectedListViewItems = list.SelectedItems;
            var items = selectedListViewItems.Cast<MyImage>();
            foreach (var item in items)
                _playlistManager.AddElem(name, item.Path);
            _playlistManager.RefreshPlaylists(treePlaylist, fgList);
        }
        public void TreePlaylist_DoubleClick(object sender, MouseButtonEventArgs e,
            TreeView treePlaylist)
        {
            if (treePlaylist.SelectedItem.GetType() != typeof (MyMedia))
                return;
            MyMedia item = (MyMedia) treePlaylist.SelectedItem;
            try
            {
                Console.WriteLine("Double Click Playing : " + item.Path);
                Media.Source = new Uri(item.Path);
                MediaPlayNormal();
            }
            catch (Exception err)
            {
                Console.WriteLine("Error:" + err);
            }
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
                        PlaylistVisibility = Visibility.Visible;
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
        public void Playlist_StartRandom(object sender, RoutedEventArgs e,
            TreeView treePlaylist)
        {
            Playlist_Start(sender, e, treePlaylist);
            _playlistManager.RandomCurrentPlaylist();
        }
        public void Playlist_Rename(object sender, RoutedEventArgs e,
            TreeView treePlaylist, String fgList)
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
                        String newName = MyDialog.Prompt("Change playlist name", "Enter the new name :", MyDialog.Size.Normal);
                        if (String.IsNullOrEmpty(newName))
                        {
                            MessageBox.Show("Incorrect name", "Error renaming playlist", MessageBoxButton.OK);
                            return;
                        }
                        _playlistManager.RenamePlaylist(item.Text, newName);
                        _playlistManager.RefreshPlaylists(treePlaylist, fgList);
                    }
                }
            }
        }
        public void Playlist_Delete(object sender, RoutedEventArgs e,
            TreeView treePlaylist, String fgList)
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
                        _playlistManager.DeletePlaylist(item.Text);
                        _playlistManager.RefreshPlaylists(treePlaylist, fgList);
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
        #endregion Event Handler
    }
}
