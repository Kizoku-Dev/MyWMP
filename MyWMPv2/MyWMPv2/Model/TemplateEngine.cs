using System;
using Antlr4.StringTemplate;
using MyWMPv2.Utilities;

namespace MyWMPv2.Model
{
    class TemplateEngine : ObservableObject
    {
        private readonly Template _colorTop1 = new Template("<arg>");
        private readonly Template _colorTop2 = new Template("<arg>");
        private readonly Template _colorTop3 = new Template("<arg>");
        private readonly Template _title = new Template("<arg>");
        private readonly Template _theme1 = new Template("<arg>");
        private readonly Template _theme2 = new Template("<arg>");
        private readonly Template _backgroundImage = new Template("<arg>");
        private readonly Template _music = new Template("<arg>");
        private readonly Template _video = new Template("<arg>");
        private readonly Template _image = new Template("<arg>");
        private readonly Template _playlist = new Template("<arg>");
        private readonly Template _listMusicFilename = new Template("<arg>");
        private readonly Template _listMusicTitle = new Template("<arg>");
        private readonly Template _listMusicAlbum = new Template("<arg>");
        private readonly Template _listMusicAuthor = new Template("<arg>");
        private readonly Template _listMusicLength = new Template("<arg>");
        private readonly Template _listVideoFilename = new Template("<arg>");
        private readonly Template _listVideoHeight = new Template("<arg>");
        private readonly Template _listVideoWidth = new Template("<arg>");
        private readonly Template _listVideoLength = new Template("<arg>");
        private readonly Template _listImageFilename = new Template("<arg>");
        private readonly Template _listImagePath = new Template("<arg>");
        private readonly Template _volume = new Template("<arg>");

        public String ColorTop1 { get { return _colorTop1.Render(); } set { } }
        public String ColorTop2 { get { return _colorTop2.Render(); } set { } }
        public String ColorTop3 { get { return _colorTop3.Render(); } set { } }
        public String Title { get { return _title.Render(); } set { } }
        public String Theme1 { get { return _theme1.Render(); } set { } }
        public String Theme2 { get { return _theme2.Render(); } set { } }
        public String BackgroundImage { get { return _backgroundImage.Render(); } set { } }
        public String Music { get { return _music.Render(); } set { } }
        public String Video { get { return _video.Render(); } set { } }
        public String Image { get { return _image.Render(); } set { } }
        public String Playlist { get { return _playlist.Render(); } set { } }
        public String ListMusicFilename { get { return _listMusicFilename.Render(); } set { } }
        public String ListMusicTitle { get { return _listMusicTitle.Render(); } set { } }
        public String ListMusicAlbum { get { return _listMusicAlbum.Render(); } set { } }
        public String ListMusicAuthor { get { return _listMusicAuthor.Render(); } set { } }
        public String ListMusicLength { get { return _listMusicLength.Render(); } set { } }
        public String ListVideoFilename { get { return _listVideoFilename.Render(); } set { } }
        public String ListVideoHeight { get { return _listVideoHeight.Render(); } set { } }
        public String ListVideoWidth { get { return _listVideoWidth.Render(); } set { } }
        public String ListVideoLength { get { return _listVideoLength.Render(); } set { } }
        public String ListImageFilename { get { return _listImageFilename.Render(); } set { } }
        public String ListImagePath { get { return _listImagePath.Render(); } set { } }
        public String Volume { get { return _volume.Render(); } set { } }
        
        /*
         * Theme 1
         */
        public void SetNormalTheme()
        {
            CleanTemplates();
            _colorTop1.Add("arg", "#51B6FF");
            _colorTop2.Add("arg", "#3A75FF");
            _colorTop3.Add("arg", "#A8DAFF");
            _title.Add("arg", "MyWindowsMediaPlayerV2");
            _theme1.Add("arg", "Theme 1");
            _theme2.Add("arg", "Theme 2");
            _backgroundImage.Add("arg", "Resources/wmpbigtasti.png");

            _music.Add("arg", "Music");
            _video.Add("arg", "Video");
            _image.Add("arg", "Image");
            _playlist.Add("arg", "Playlist");

            _listMusicFilename.Add("arg", "Filename");
            _listMusicTitle.Add("arg", "Title");
            _listMusicAlbum.Add("arg", "Album");
            _listMusicAuthor.Add("arg", "Author");
            _listMusicLength.Add("arg", "Length");

            _listVideoFilename.Add("arg", "Filename");
            _listVideoHeight.Add("arg", "Height");
            _listVideoWidth.Add("arg", "Width");
            _listVideoLength.Add("arg", "Length");

            _listImageFilename.Add("arg", "Filename");
            _listImagePath.Add("arg", "Path");
            _volume.Add("arg", "Volume");
            NotifyPropertyChanged();
        }

        /*
         * Theme 2
         */
        public void Set1337Theme()
        {
            CleanTemplates();
            _colorTop1.Add("arg", "#59EA78");
            _colorTop2.Add("arg", "#08630A");
            _colorTop3.Add("arg", "#4EFF3A");
            _title.Add("arg", "MYW1ND0W5M3D14PL4Y3RV2");
            _theme1.Add("arg", "7H3M3 1");
            _theme2.Add("arg", "7H3M3 2");
            _backgroundImage.Add("arg", "Resources/wmp1337.png");

            _music.Add("arg", "MU51C");
            _video.Add("arg", "V1D30");
            _image.Add("arg", "1M463");
            _playlist.Add("arg", "PL4YL157");

            _listMusicFilename.Add("arg", "F1L3N4M3");
            _listMusicTitle.Add("arg", "717L3");
            _listMusicAlbum.Add("arg", "4L8UM");
            _listMusicAuthor.Add("arg", "4U7H0R");
            _listMusicLength.Add("arg", "L3N67H");

            _listVideoFilename.Add("arg", "F1L3N4M3");
            _listVideoHeight.Add("arg", "H316H7");
            _listVideoWidth.Add("arg", "W1D7H");
            _listVideoLength.Add("arg", "L3N67H");

            _listImageFilename.Add("arg", "F1L3N4M3");
            _listImagePath.Add("arg", "P47H");
            _volume.Add("arg", "V0LUM3");
            NotifyPropertyChanged();
        }

        private void NotifyPropertyChanged()
        {
            OnPropertyChanged("ColorTop1");
            OnPropertyChanged("ColorTop2");
            OnPropertyChanged("ColorTop3");
            OnPropertyChanged("Title");
            OnPropertyChanged("Theme1");
            OnPropertyChanged("Theme2");
            OnPropertyChanged("BackgroundImage");

            OnPropertyChanged("Music");
            OnPropertyChanged("Video");
            OnPropertyChanged("Image");
            OnPropertyChanged("Playlist");

            OnPropertyChanged("ListMusicFilename");
            OnPropertyChanged("ListMusicTitle");
            OnPropertyChanged("ListMusicAlbum");
            OnPropertyChanged("ListMusicAuthor");
            OnPropertyChanged("ListMusicLength");

            OnPropertyChanged("ListVideoFilename");
            OnPropertyChanged("ListVideoHeight");
            OnPropertyChanged("ListVideoWidth");
            OnPropertyChanged("ListVideoLength");

            OnPropertyChanged("ListImageFilename");
            OnPropertyChanged("ListImagePath");
            OnPropertyChanged("Volume");
        }

        private void CleanTemplates()
        {
            _colorTop1.Remove("arg");
            _colorTop2.Remove("arg");
            _colorTop3.Remove("arg");
            _title.Remove("arg");
            _theme1.Remove("arg");
            _theme2.Remove("arg");
            _backgroundImage.Remove("arg");

            _music.Remove("arg");
            _video.Remove("arg");
            _image.Remove("arg");
            _playlist.Remove("arg");

            _listMusicFilename.Remove("arg");
            _listMusicTitle.Remove("arg");
            _listMusicAlbum.Remove("arg");
            _listMusicAuthor.Remove("arg");
            _listMusicLength.Remove("arg");

            _listVideoFilename.Remove("arg");
            _listVideoHeight.Remove("arg");
            _listVideoWidth.Remove("arg");
            _listVideoLength.Remove("arg");

            _listImageFilename.Remove("arg");
            _listImagePath.Remove("arg");
            _volume.Remove("arg");
        }
    }
}
