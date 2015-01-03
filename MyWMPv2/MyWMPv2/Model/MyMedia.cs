using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MyWMPv2.Utilities;

namespace MyWMPv2.Model
{
    class MyMedia : IMyMedia
    {
        private ICommand _deleteSelfCommand;
        private readonly PlaylistManager _parent;

        public String Playlist { set; get; }
        public String Path { set; get; }
        public String Filename { set; get; }
        public String Name { set; get; }
        public String PlaylistPath { get; set; }
        public Color FgList { get; set; }

        public MyMedia(ref PlaylistManager parent)
        {
            _parent = parent;
        }

        public ICommand DeleteSelfCommand
        {
            get
            {
                if (_deleteSelfCommand == null)
                    _deleteSelfCommand = new DelegateCommand(DeleteSelf);
                return _deleteSelfCommand;
            }
        }

        private void DeleteSelf(object sender)
        {
            String path = (sender as String);
            String[] array = path.Split(new string[] { "<my1337haxorseparator>" }, StringSplitOptions.None);
            array[1] = array[1].Replace(" ", "%20").Replace("\\", "/");
            _parent.DeleteElem(array[0], array[1]);
        }
    }
}
