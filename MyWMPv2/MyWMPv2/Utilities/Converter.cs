using System;
using System.Windows.Media;

namespace MyWMPv2.Utilities
{
    class Converter
    {
        public static Color StringToColor(String value)
        {
            Color col = new Color();
            col = Colors.Black;
            var convertFromString = ColorConverter.ConvertFromString(value);
            if (convertFromString != null)
                col = (Color)convertFromString;
            return col;
        }
    }
}
