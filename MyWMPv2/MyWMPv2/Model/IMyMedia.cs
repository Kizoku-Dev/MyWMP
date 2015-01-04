using System;
using System.Windows.Media;

namespace MyWMPv2.Model
{
    interface IMyMedia
    {
        String Path { set; get; }
        String Filename { set; get; }
        Color FgList { get; set; }
    }
}
