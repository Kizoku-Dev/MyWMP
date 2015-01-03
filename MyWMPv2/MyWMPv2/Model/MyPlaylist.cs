using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace MyWMPv2.Model
{
    class MyPlaylist
    {
        public String Name { get; set; }
        public List<MyMedia> Medias { get; set; }
        public Color FgList { get; set; }
    }
}
