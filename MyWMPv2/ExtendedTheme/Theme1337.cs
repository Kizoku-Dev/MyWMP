using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ExtendedTheme
{
    [Export(typeof(MyWMPv2.Model.ITheme))]
    [ExportMetadata("Theme", "1337")]
    public class Theme1337 : MyWMPv2.Model.ITheme
    {
        private readonly List<String> _elems = new List<String>()
        {
            "#08630A", "#59EA78", "#4EFF3A", "black", "white", "white", // window color
            "black", "MYW1ND0W5M3D14PL4Y3RV2", "Resources/Images/wmp1337.png", // title and color and logo
            "white", "black", "CH4N63 7H3M3", "MU51C", "V1D30", "1M463", "PL4YL157", // button
            "black", "green", // list
            "F1L3N4M3", "717L3", "4L8UM", "4U7H0R", "L3N67H", // list music
            "F1L3N4M3", "H316H7", "W1D7H", "L3N67H", // list video
            "F1L3N4M3", "P47H", // list image
            "green", "darkgreen", // slider color
            "Resources/Images/play-circle-black.png", // media play
            "Resources/Images/pause-black.png", // media pause
            "Resources/Images/stop-black.png", // media stop
            "Resources/Images/file-black.png", // media open
            "Resources/Images/x-black.png", // close
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
