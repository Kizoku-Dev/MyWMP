using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ExtendedTheme
{
    [Export(typeof(MyWMPv2.Model.ITheme))]
    [ExportMetadata("Theme", "light")]
    class ThemeLight : MyWMPv2.Model.ITheme
    {
        private readonly List<String> _elems = new List<String>()
        {
            "white", "white", "#white", "white", "white", "white", // window color
            "black", "MyWindowsMediaPlayerV2", "Resources/Images/wmpwhitemage.png", // title and color and logo
            "white", "black", "Change Theme", "Music", "Video", "Image", "Playlist", // button
            "white", "black", // list
            "Filename", "Title", "Album", "Author", "Length", // list music
            "Filename", "Height", "Width", "Length", // list video
            "Filename", "Path", // list image
            "black", "black", // slider color
            "Resources/Images/play-circle-black.png", // media play
            "Resources/Images/pause-black.png", // media pause
            "Resources/Images/stop-black.png", // media stop
            "Resources/Images/file-black.png", // media open
            "Resources/Images/x-black.png" , // close
            "Resources/Images/backward-black.png", // media prev
            "Resources/Images/forward-black.png", // media next
            "Resources/Images/loop-black.png", // media loop1
            "Resources/Images/loop2-black.png", // media loop2
            "Resources/Images/bar-black.png" // minimize
        };

        public String GetThemeString(int index)
        {
            return _elems[index];
        }
    }
}
