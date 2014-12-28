using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWMPv2.Model
{
    interface IMyMedia
    {
        String Path { set; get; }
        String Filename { set; get; }
    }
}
