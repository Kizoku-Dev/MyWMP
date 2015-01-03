using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ExtendedTheme
{
    [Export(typeof(MyWMPv2.Model.ITheme))]
    [ExportMetadata("Theme", "Dark")]
    public class ThemeDark : MyWMPv2.Model.ITheme
    {
        private readonly List<String> _elems = new List<String>()
        {
            "black", "#121212", "#252525", "black", "#121212", "black", // window color
            "white", "MyWindowsMediaPlayerV2", "Resources/Images/wmpboo.png", // title and color and logo
            "black", "white", "Change Theme", "Music", "Video", "Image", "Playlist", // button
            "black", "white", // list
            "Filename", "Title", "Album", "Author", "Length", // list music
            "Filename", "Height", "Width", "Length", // list video
            "Filename", "Path", // list image
            "white", "#252525", // slider color
            "Resources/Images/play-circle-white.png", // media play
            "Resources/Images/pause-white.png", // media pause
            "Resources/Images/stop-white.png", // media stop
            "Resources/Images/file-white.png", // media open
            "Resources/Images/x-white.png", // close
            "Resources/Images/backward-white.png", // media prev
            "Resources/Images/forward-white.png" // media next
        };

        public String GetThemeString(int index)
        {
            return _elems[index];
        }
    }
}
