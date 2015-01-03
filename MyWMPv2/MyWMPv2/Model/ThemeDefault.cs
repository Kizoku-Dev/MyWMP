using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace MyWMPv2.Model
{
    [Export(typeof(ITheme))]
    [ExportMetadata("Theme", "Default")]
    public class ThemeDefault : ITheme
    {
        /*
         * Theme's protocol :
         * [0] = color window 1
         * [1] = color window 2
         * [2] = color window 3
         * [3] = color window 4
         * [4] = color window 5
         * [5] = color window 6
         * [6] = color title and media time foreground&
         * [7] = title
         * [8] = image logo
         * [9] = color button background
         * [10] = color button foreground
         * [11] = button change theme
         * [12] = button music
         * [13] = button video
         * [14] = button image
         * [15] = button playlist
         * [16] = color list background
         * [17] = color list foreground
         * [18] = list music filename
         * [19] = list music title
         * [20] = list music album
         * [21] = list music author
         * [22] = list music length
         * [23] = list video filename
         * [24] = list video height
         * [25] = list video width
         * [26] = list video length
         * [27] = list image filename
         * [28] = list image path
         * [29] = color slider 1
         * [30] = color slider 2
         * [31] = image play
         * [32] = image pause
         * [33] = image stop
         * [34] = image open
         * [35] = image close
         * [36] = image prev
         * [37] = image next
         */
        private List<String> _elems = new List<String>()
        {
            "#51B6FF", "#3A75FF", "#A8DAFF", "black", "white", "white", // window color
            "black", "MyWindowsMediaPlayerV2", "Resources/Images/wmpbigtasti.png", // title and color and logo
            "white", "black", "Change Theme", "Music", "Video", "Image", "Playlist", // button
            "white", "black", // list
            "Filename", "Title", "Album", "Author", "Length", // list music
            "Filename", "Height", "Width", "Length", // list video
            "Filename", "Path", // list image
            "#00BDC4", "#0070E0", // slider color
            "Resources/Images/play-circle-black.png", // media play
            "Resources/Images/pause-black.png", // media pause
            "Resources/Images/stop-black.png", // media stop
            "Resources/Images/file-black.png", // media open
            "Resources/Images/x-black.png", // close
            "Resources/Images/backward-black.png", // media prev
            "Resources/Images/forward-black.png" // media next

        };

        public List<String> Elems
        {
            get { return _elems; }
        }
        public String GetThemeString(int index)
        {
            return _elems[index];
        }
    }
}
