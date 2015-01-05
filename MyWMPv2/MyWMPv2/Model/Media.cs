using System;
using System.IO;
using System.Windows.Controls;
using MyWMPv2.Utilities;

namespace MyWMPv2.Model
{
    class Media : ObservableObject
    {
        #region Private member variables
        private Uri _source;
        private String _sourceString;
        private MediaState _state;
        private String _progress;
        private String _position;
        private String _positionMax;
        private Double _opacity;
        #endregion Private member variables

        #region Public member variables
        public Uri Source
        {
            get { return _source; }
            set
            {
                _source = value;
                SourceString = Path.GetFileNameWithoutExtension(_source.OriginalString);
                OnPropertyChanged("Source");
            }
        }
        public String SourceString
        {
            get { return _sourceString; }
            set
            {
                if (value.Length > 80)
                    _sourceString = "Playing : a youtube video...";
                else
                    _sourceString = "Playing : \""+value+"\"";
                OnPropertyChanged("SourceString");
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
                    _progress = "Buffering : " + value + "%";
                OnPropertyChanged("Progress");
            }
        }
        public String Position
        {
            get { return _position; }
            set
            {
                if (value == "00:00")
                    _position = "";
                else
                    _position = value + "   /";
                OnPropertyChanged("Position");
            }
        }
        public String PositionMax
        {
            get { return _positionMax; }
            set
            {
                if (value == "00:00")
                    _positionMax = "";
                else
                    _positionMax = value;
                OnPropertyChanged("PositionMax");
            }
        }
        public Double Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                OnPropertyChanged("Opacity");
            }
        }
        #endregion Public member variables
    }
}
