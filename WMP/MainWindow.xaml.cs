using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace WMP
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NameEntering nameEntering;

        public MainWindow()
        {
            InitializeComponent();
            BitmapImage bm = new BitmapImage();
            bm.BeginInit();
            bm.UriSource = new Uri("C:\\Users\\ovoyan_s\\Desktop\\WMP\\fall.jpg");
            bm.DecodePixelHeight= (int)this.mainWindow.Height;
            bm.DecodePixelWidth = (int)this.mainWindow.Width;
            bm.EndInit();
            this.imageDeFond.Source = bm;
            this.imageDeFond.Height = (int)this.mainWindow.Height;
            this.imageDeFond.Width = (int)this.mainWindow.Width;
            nameEntering = new NameEntering("", "", "", this);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.mediaElement.Volume = this.soundSlider.Value / 10;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.mediaElement.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.mediaElement.Pause();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.mediaElement.Stop();
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            this.mediaSlider.Value = this.mediaElement.Position.TotalSeconds;
            String str = "";
            int num = (int)this.mediaElement.Position.TotalHours / 3600;
            str = str + num.ToString() + ":";
            num = (int)this.mediaElement.Position.TotalSeconds / 60;
            str = str + num.ToString() + ":";
            num = (int)this.mediaElement.Position.TotalSeconds % 60;
            str = str + num.ToString();
            this.currentLabel.Content = str;
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan ts = TimeSpan.FromSeconds(e.NewValue);
            this.mediaElement.Position = ts;
        }

        private void mediaElement_Opened(object sender, RoutedEventArgs e)
        {
            if (mediaElement.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = TimeSpan.FromMilliseconds(mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds);
                mediaSlider.Maximum = ts.TotalSeconds;
                String str = "";
                int num = (int)ts.TotalSeconds / 3600;
                str = str + num.ToString() + ":";
                num = (int)ts.TotalSeconds / 60;
                str = str + num.ToString() + ":";
                num = (int)ts.TotalSeconds % 60;
                str = str + num.ToString();
                this.totalLabel.Content = str;
            }
        }

        private void open_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.AddExtension = true;
            fileDialog.DefaultExt = ".";
            fileDialog.CheckPathExists = true;
            fileDialog.ShowDialog();

            try
            {
                this.mediaElement.Source = new Uri(fileDialog.FileName);
            }
            catch
            {
                new NullReferenceException("Error");
            }

            System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();
            dt.Tick += new EventHandler(timer_Tick);
            dt.Interval = new TimeSpan(0, 0, 1);
            dt.Start();
        }

        private void uploading_of_medias_list()
        {

        }

        private void treeRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.generalMenu();
        }

        private void generalMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            //test
            this.createMenuItemToContextMenu(contextMenu, "Create Playlist");
            ((MenuItem)contextMenu.Items.GetItemAt(0)).Click += new RoutedEventHandler(this.create_playlist);
            contextMenu.IsOpen = true;
            contextMenu.Visibility = System.Windows.Visibility.Visible;
        }

        private void create_playlist(object sender, EventArgs e)
        {
            if (this.playlistExists("New Playlist") == true)
                MessageBox.Show("There is arleady a playlist with the \"New Playlist\" name");
            else
            {
                if (System.IO.File.Exists("C:\\Users\\ovoyan_s\\Desktop\\lol.txt") == false)
                {
                    FileStream newFile = System.IO.File.Create("C:\\Users\\ovoyan_s\\Desktop\\lol.txt");
                    newFile.Close();
                }
                if (System.IO.File.Exists("C:\\Users\\ovoyan_s\\Desktop\\lol.txt") == true)
                {
                    MessageBox.Show("Step 1");
                    StringBuilder sb = new StringBuilder();
                    using (StringWriter stringWriter = new StringWriter(sb))
                    {
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        settings.IndentChars = "\t";
                        using (XmlWriter textWriter = XmlWriter.Create(sb, settings))
                        {
                            MessageBox.Show("Step 3");
                            textWriter.WriteStartElement("Playlist");
                            textWriter.WriteElementString("Time", DateTime.Now.ToString());
                            textWriter.WriteEndElement();
                        }
                    }
                    using (StreamWriter streamWriter = new StreamWriter("C:\\Users\\ovoyan_s\\Desktop\\lol.txt", true))
                    {
                        streamWriter.WriteLine(sb.ToString());
                        streamWriter.Close();
                    }
                    TreeViewItem tvi = new TreeViewItem();
                    tvi.Header = "New Playlist";
                    this.libList.Items.Add(tvi);
                }
            }
        }

        private bool playlistExists(string name)
        {
            int counter = 0;

            while (counter < this.libList.Items.Count)
            {
                if ((string)((TreeViewItem)this.libList.Items.GetItemAt(counter)).Header == name)
                    return (true);
                ++counter;
            }
            return (false);
        }

        private void create_media(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("C:\\Users\\ovoyan_s\\Desktop\\lol.txt") == false)
                System.IO.File.Create("C:\\Users\\ovoyan_s\\Desktop\\lol.txt");
            if (System.IO.File.Exists("C:\\Users\\ovoyan_s\\Desktop\\lol.txt") == true)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.AddExtension = true;
                fileDialog.DefaultExt = ".";
                fileDialog.CheckPathExists = true;
                fileDialog.ShowDialog();

                TreeViewItem item = new TreeViewItem();

                item.Header = fileDialog.SafeFileName;
                ((TreeViewItem)this.libList.SelectedItem).Items.Add(item);

                System.IO.StreamWriter streamWriter = new System.IO.StreamWriter("C:\\Users\\ovoyan_s\\Desktop\\lol.txt", true);
                streamWriter.WriteLine("Media : " + fileDialog.SafeFileName + " " + fileDialog.FileName);
                streamWriter.Close();
            }

        }

        private void delete_playlist(object sender, EventArgs e)
        {
            this.libList.Items.RemoveAt(this.libList.Items.IndexOf((TreeViewItem)this.libList.SelectedItem));
        }

        private void delete_media(object sender, EventArgs e)
        {
            TreeViewItem parentItem = (TreeViewItem)((TreeViewItem)this.libList.SelectedItem).Parent;

            parentItem.Items.RemoveAt(parentItem.Items.IndexOf((TreeViewItem)this.libList.SelectedItem));
        }

        private void rename_playlist(object sender, EventArgs e)
        {
            this.nameEntering.setNames("Enter your playlist name", "New PlayList", "Change your Playlist Name !");
            if (this.nameEntering.Visibility == System.Windows.Visibility.Collapsed
                || this.nameEntering.Visibility == System.Windows.Visibility.Hidden)
                this.nameEntering.Visibility = System.Windows.Visibility.Visible;
        }

        private void rename_media(object sender, EventArgs e)
        {
            this.nameEntering.setNames("Enter your media name", "New Media", "Change your Media Name !");
            if (this.nameEntering.Visibility == System.Windows.Visibility.Collapsed
                || this.nameEntering.Visibility == System.Windows.Visibility.Hidden)
                this.nameEntering.Visibility = System.Windows.Visibility.Visible;
        }

        private void playlist_properties(object sender, EventArgs e)
        {

        }

        private void createMenuItem(MenuItem menuItem, String nameOfNewItem)
        {
            MenuItem itemToAdd = new MenuItem();

            itemToAdd.Header = nameOfNewItem;
            menuItem.Items.Add(itemToAdd);
        }

        private void createMenuItemToContextMenu(ContextMenu contextMenu, String nameOfNewItem)
        {
            MenuItem itemToAdd = new MenuItem();

            itemToAdd.Header = nameOfNewItem;
            contextMenu.Items.Add(itemToAdd);
        }

        public void renaming(object sender, EventArgs e)
        {
            ((TreeViewItem)this.libList.SelectedItem).Header = this.nameEntering.editToUse.Text;
            this.nameEntering.Visibility = System.Windows.Visibility.Hidden;
        }

        public void media_properties(object sender, EventArgs e)
        {

        }

        private void treeEventHandler(object sender, EventArgs e)
        {
            if (this.ifItemHaveParent((TreeViewItem)this.libList.SelectedItem) == true)
            {
                ContextMenu contextMenu = new ContextMenu();

                this.createMenuItemToContextMenu(contextMenu, "Media");
                this.createMenuItemToContextMenu(contextMenu, "Play Media");
                this.createMenuItemToContextMenu(contextMenu, "Properties");
                this.createMenuItem((MenuItem)contextMenu.Items.GetItemAt(0), "Rename Media");
                this.createMenuItem((MenuItem)contextMenu.Items.GetItemAt(0), "Delete Media");
                ((MenuItem)contextMenu.Items.GetItemAt(1)).Click += new RoutedEventHandler(this.media_properties);
                ((MenuItem)((MenuItem)contextMenu.Items.GetItemAt(0)).Items.GetItemAt(0)).Click += new RoutedEventHandler(this.rename_media);
                ((MenuItem)((MenuItem)contextMenu.Items.GetItemAt(0)).Items.GetItemAt(1)).Click += new RoutedEventHandler(this.delete_media);
                contextMenu.IsOpen = true;
                contextMenu.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ContextMenu contextMenu = new ContextMenu();

                this.createMenuItemToContextMenu(contextMenu, "Playlist");
                this.createMenuItemToContextMenu(contextMenu, "Create Media");
                this.createMenuItemToContextMenu(contextMenu, "Play Playlist");
                this.createMenuItemToContextMenu(contextMenu, "Properties");
                this.createMenuItem((MenuItem)contextMenu.Items.GetItemAt(0), "Rename Playlist");
                this.createMenuItem((MenuItem)contextMenu.Items.GetItemAt(0), "Delete Playlist");
                ((MenuItem)contextMenu.Items.GetItemAt(1)).Click += new RoutedEventHandler(this.create_media);
                ((MenuItem)contextMenu.Items.GetItemAt(2)).Click += new RoutedEventHandler(this.playlist_properties);
                ((MenuItem)((MenuItem)contextMenu.Items.GetItemAt(0)).Items.GetItemAt(0)).Click += new RoutedEventHandler(this.rename_playlist);
                ((MenuItem)((MenuItem)contextMenu.Items.GetItemAt(0)).Items.GetItemAt(1)).Click += new RoutedEventHandler(this.delete_playlist);
                contextMenu.IsOpen = true;
                contextMenu.Visibility = System.Windows.Visibility.Visible;
            }
        }
        
        private bool ifItemHaveParent(TreeViewItem treeViewItem)
        {
            if (treeViewItem.Parent == this.libList)
                return (false);
            else
                return (true);
        }
    }
}
