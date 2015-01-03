using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedTheme
{
    [Export(typeof(MyWMPv2.Model.ITheme))]
    [ExportMetadata("Theme", "Doge")]
    class ThemeDoge : MyWMPv2.Model.ITheme
    {
        private readonly List<String> _elems = new List<String>()
        {
            "#CA983F", "#D7B868", "#D0C79E", "black", "#BAA85C", "#C5B881", // window color
            "#AC5695", "WOW", "Resources/Images/wmpdoge.png", // title and color and logo
            "white", "#338DA8", "such player", "amaze", "do click", "much button", "so taste", // button
            "#CCB67C", "#F11422", // list
            "very cool", "i am tired", "plz", "such author", "wow", // list music
            "very cool", "such awesome", "follow your dream", "wow", // list video
            "very cool", "sweet", // list image
            "#668B3B", "#C38C4C", // slider color
            "Resources/Images/play-circle-black.png", // media play
            "Resources/Images/pause-black.png", // media pause
            "Resources/Images/stop-black.png", // media stop
            "Resources/Images/file-black.png", // media open
            "Resources/Images/x-black.png", // close
            "Resources/Images/backward-black.png", // media prev
            "Resources/Images/forward-black.png", // media next
            "Resources/Images/loop-black.png", // media loop1
            "Resources/Images/loop2-black.png" // media loop2
        };

        public String GetThemeString(int index)
        {
            return _elems[index];
        }
    }
}
