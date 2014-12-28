using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MyWMPv2.Utilities;

namespace MyWMPv2.Model
{
    class Media : ObservableObject
    {
        private Uri _source;
        private MediaState _state;

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
    }
}
