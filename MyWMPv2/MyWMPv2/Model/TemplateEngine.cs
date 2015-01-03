using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Windows;
using Antlr4.StringTemplate;
using MyWMPv2.Utilities;

namespace MyWMPv2.Model
{
    class TemplateEngine : ObservableObject
    {
        [ImportMany]
        IEnumerable<Lazy<ITheme, IThemeData>> _themes;

        private readonly List<Template> _elems;
        private readonly List<ThemeElem> _currentTheme;
        private const int _nbElem = 38;
        private readonly String _themePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyWMPv2\\Themes";
        private readonly ThemeDefault _default = new ThemeDefault();
        public TemplateEngine()
        {
            if (!Directory.Exists(_themePath))
                Directory.CreateDirectory(_themePath);
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(TemplateEngine).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(_themePath, "*.dll"));
            var container = new CompositionContainer(catalog);
            try { container.ComposeParts(this); }
            catch (CompositionException compositionException) { Console.WriteLine(compositionException.ToString()); }
            _elems = new List<Template>();
            _currentTheme = new List<ThemeElem>();
            for (int i = 0; i < _nbElem; ++i)
            {
                _elems.Add(new Template("<arg>"));
                _currentTheme.Add(new ThemeElem() { Value = "", DefaultValue = "" });
            }
        }

        public String BgWindow1 { get { return _elems[0].Render(); } set { } }
        public String BgWindow2 { get { return _elems[1].Render(); } set { } }
        public String BgWindow3 { get { return _elems[2].Render(); } set { } }
        public String BgWindow4 { get { return _elems[3].Render(); } set { } }
        public String BgWindow5 { get { return _elems[4].Render(); } set { } }
        public String BgWindow6 { get { return _elems[5].Render(); } set { } }
        public String FgTitle { get { return _elems[6].Render(); } set { } }
        public String Title { get { return _elems[7].Render(); } set { } }
        public String ImgLogo { get { return _elems[8].Render(); } set { } }
        public String BgButton { get { return _elems[9].Render(); } set { } }
        public String FgButton { get { return _elems[10].Render(); } set { } }
        public String ChangeTheme { get { return _elems[11].Render(); } set { } }
        public String Music { get { return _elems[12].Render(); } set { } }
        public String Video { get { return _elems[13].Render(); } set { } }
        public String Image { get { return _elems[14].Render(); } set { } }
        public String Playlist { get { return _elems[15].Render(); } set { } }
        public String BgList { get { return _elems[16].Render(); } set { } }
        public String FgList { get { return _elems[17].Render(); } set { } }
        public String ListMusicFilename { get { return _elems[18].Render(); } set { } }
        public String ListMusicTitle { get { return _elems[19].Render(); } set { } }
        public String ListMusicAlbum { get { return _elems[20].Render(); } set { } }
        public String ListMusicAuthor { get { return _elems[21].Render(); } set { } }
        public String ListMusicLength { get { return _elems[22].Render(); } set { } }
        public String ListVideoFilename { get { return _elems[23].Render(); } set { } }
        public String ListVideoHeight { get { return _elems[24].Render(); } set { } }
        public String ListVideoWidth { get { return _elems[25].Render(); } set { } }
        public String ListVideoLength { get { return _elems[26].Render(); } set { } }
        public String ListImageFilename { get { return _elems[27].Render(); } set { } }
        public String ListImagePath { get { return _elems[28].Render(); } set { } }
        public String ColorSlider1 { get { return _elems[29].Render(); } set { } }
        public String ColorSlider2 { get { return _elems[30].Render(); } set { } }
        public String ImgPlay { get { return _elems[31].Render(); } set { } }
        public String ImgPause { get { return _elems[32].Render(); } set { } }
        public String ImgStop { get { return _elems[33].Render(); } set { } }
        public String ImgOpen { get { return _elems[34].Render(); } set { } }
        public String ImgClose { get { return _elems[35].Render(); } set { } }
        public String ImgPrev { get { return _elems[36].Render(); } set { } }
        public String ImgNext { get { return _elems[37].Render(); } set { } }
        public List<ThemeElem> CurrentTheme { get { return _currentTheme; } }
        
        public void SetTheme(String theme)
        {
            CleanTemplates();
            bool ok = false;
            foreach (Lazy<ITheme, IThemeData> p in _themes)
            {
                if (p.Metadata.Theme.Equals(theme))
                {
                    ok = true;
                    try
                    {
                        for (int i = 0; i < _nbElem; ++i)
                        {
                            _currentTheme[i].Value = p.Value.GetThemeString(i);
                            _currentTheme[i].DefaultValue = _default.Elems[i];
                            _elems[i].Add("arg", _currentTheme[i].Value);
                        }
                    }
                    catch (Exception e) { Console.WriteLine("Error"+e); }
                }
            }
            if (!ok)
            {
                MessageBox.Show("Theme not found.", "Error loading theme");
                Console.WriteLine("Error loading theme");
                SetTheme("Default");
                return;
            }
            NotifyPropertyChanged();
        }

        private void NotifyPropertyChanged()
        {
            OnPropertyChanged("BgWindow1");
            OnPropertyChanged("BgWindow2");
            OnPropertyChanged("BgWindow3");
            OnPropertyChanged("BgWindow4");
            OnPropertyChanged("BgWindow5");
            OnPropertyChanged("BgWindow6");
            OnPropertyChanged("FgTitle");
            OnPropertyChanged("Title");
            OnPropertyChanged("ImgLogo");
            OnPropertyChanged("BgButton");
            OnPropertyChanged("FgButton");
            OnPropertyChanged("ChangeTheme");
            OnPropertyChanged("Music");
            OnPropertyChanged("Video");
            OnPropertyChanged("Image");
            OnPropertyChanged("Playlist");
            OnPropertyChanged("BgList");
            OnPropertyChanged("FgList");
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
            OnPropertyChanged("ColorSlider1");
            OnPropertyChanged("ColorSlider2");
            OnPropertyChanged("ImgPlay");
            OnPropertyChanged("ImgPause");
            OnPropertyChanged("ImgStop");
            OnPropertyChanged("ImgOpen");
            OnPropertyChanged("ImgClose");
            OnPropertyChanged("ImgPrev");
            OnPropertyChanged("ImgNext");
        }

        private void CleanTemplates()
        {
            foreach (Template elem in _elems)
                elem.Remove("arg");
            for (int i = 0; i < _nbElem; ++i)
            {
                _currentTheme[i].Value = "";
                _currentTheme[i].DefaultValue = "";
            }
        }
    }
}
