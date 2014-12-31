using System;

namespace MyWMPv2.Model
{
    class MyMusic : IMyMedia
    {
        public String Path { set; get; }
        public String Filename { set; get; }
        public String Title { set; get; }
        public String Album { set; get; }
        public String Author { set; get; }
        public TimeSpan Length { set; get; }
    }
}
