using System;
using System.Windows.Controls;
using MyWMPv2.Utilities;

namespace MyWMPv2.Model
{
    class Media : ObservableObject
    {
        private Uri _source;
        private MediaState _state;
        private String _progress;

        public Uri Source
        {
            get { return _source; }
            set
            {
                _source = value;
                OnPropertyChanged("Source");
            }
        }
        public MediaState State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged("State");
            }
        }
        public String Progress
        {
            get { return _progress; }
            set
            {
                if (value == "0")
                    _progress = "";
                else
                    _progress = "Buffering : "+value+"%";
                OnPropertyChanged("Progress");
            }
        }
    }
}
