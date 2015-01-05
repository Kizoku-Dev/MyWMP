using System;
using System.Windows.Media;

namespace MyWMPv2.Model
{
    class MyImage : IMyMedia
    {
        public String Path { set; get; }
        public String Filename { set; get; }
        public Color FgList { get; set; }
    }
}
