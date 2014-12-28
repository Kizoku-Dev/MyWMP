using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWMPv2.Model
{
    class MyVideo : IMyMedia
    {
        public String Path { set; get; }
        public String Filename { set; get; }
        public int Height { set; get; }
        public int Width { set; get; }
        public TimeSpan Length { set; get; }
    }
}
