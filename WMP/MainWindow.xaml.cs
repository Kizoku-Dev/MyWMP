using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WMP
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            media1.LoadedBehavior = MediaState.Manual;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            media1.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            media1.Stop();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            media1.Pause();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media Files (*.*)|*.*";
            ofd.ShowDialog();

            try { media1.Source = new Uri(ofd.FileName); }
            catch { new NullReferenceException("Error"); }

            System.Windows.Threading.DispatcherTimer dispatcherTime = new System.Windows.Threading.DispatcherTimer();
            dispatcherTime.Tick += new EventHandler(timer_Tick);
            dispatcherTime.Interval = new TimeSpan(0, 0, 1);
            dispatcherTime.Start();
        }

        void    timer_Tick(object sender, EventArgs e)
        {
            sliderMedia.Value = media1.Position.TotalSeconds;
        }

        private void slideMedia_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan ts = TimeSpan.FromSeconds(e.NewValue);
            media1.Position = ts;
        }

        private void slideVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media1.Volume = sliderVolume.Value;
        }

        private void media1_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (media1.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = TimeSpan.FromMilliseconds(media1.NaturalDuration.TimeSpan.TotalMilliseconds);
                sliderMedia.Maximum = ts.TotalSeconds;
            }
        }

    }
}
