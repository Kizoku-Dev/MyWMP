using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWMPv2.Model
{
    class MyImage : IMyMedia
    {
        public String Path { set; get; }
        public String Filename { set; get; }
    }
}
