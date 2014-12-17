using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WMP
{
    class ViewModel
    {
        private bool _slideChanged;

        public void btnPlay_Click(object sender, RoutedEventArgs e,
            MediaElement media1, Slider sliderMedia)
        {
            media1.Play();
            MediaClock clock = media1.Clock;
            if (clock != null)
            {
                if (clock.IsPaused)
                    clock.Controller.Resume();
                else
                    clock.Controller.Pause();
            }
            else
            {
                MediaTimeline timeline = new MediaTimeline(media1.Source);
                clock = timeline.CreateClock();
                clock.CurrentTimeInvalidated += new EventHandler((sender1, e1) => Clock_CurrentTimeInvalidated(sender, e, media1, sliderMedia));
                media1.Clock = clock;
            }
        }

        public void Clock_CurrentTimeInvalidated(object sender, EventArgs e,
            MediaElement media1, Slider sliderMedia)
        {
            if (media1.Clock == null || this._slideChanged)
                return;
            if (media1.Clock.CurrentTime != null)
                sliderMedia.Value = media1.Clock.CurrentTime.Value.TotalMilliseconds;
        }

        public void btnStop_Click(object sender, RoutedEventArgs e,
            MediaElement media1)
        {
            media1.Stop();
            media1.Clock = null;
        }

        public void btnOpen_Click(object sender, RoutedEventArgs e,
            MediaElement media1, Slider sliderMedia)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media Files (*.*)|*.*";
            ofd.ShowDialog();

            try { media1.Source = new Uri(ofd.FileName); }
            catch { new NullReferenceException("Error"); }

            this.btnStop_Click(sender, e, media1);
            this.btnPlay_Click(sender, e, media1, sliderMedia);
        }

        public void slideMedia_MouseDown(object sender, RoutedEventArgs e,
            MediaElement media1)
        {
            this._slideChanged = true;

        }

        public void slideMedia_MouseUp(object sender, RoutedEventArgs e,
            MediaElement media1, Slider sliderMedia)
        {
            MediaClock clock = media1.Clock;
            if (clock != null)
            {
                TimeSpan mOffset = TimeSpan.FromMilliseconds(sliderMedia.Value);
                if (clock.Controller != null) clock.Controller.Seek(mOffset, TimeSeekOrigin.BeginTime);
            }
            this._slideChanged = false;
        }

        public void media1_MediaOpened(object sender, RoutedEventArgs e,
            MediaElement media1, Slider sliderMedia)
        {
            if (media1.NaturalDuration.HasTimeSpan)
                sliderMedia.Maximum = media1.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

    }
}
