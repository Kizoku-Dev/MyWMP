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
        XmlDocument playListDocument;

        public MainWindow()
        {
            InitializeComponent();
            BitmapImage bm = new BitmapImage();
            bm.BeginInit();
            bm.UriSource = new Uri(Environment.CurrentDirectory + "\\Ressources\\fall.jpg");
            bm.DecodePixelHeight= (int)this.mainWindow.Height;
            bm.DecodePixelWidth = (int)this.mainWindow.Width;
            bm.EndInit();
            this.imageDeFond.Source = bm;
            this.imageDeFond.Height = (int)this.mainWindow.Height;
            this.imageDeFond.Width = (int)this.mainWindow.Width;
            nameEntering = new NameEntering("", "", "");
            this.playListDocument = new XmlDocument();
            string str = "";
            if (System.IO.File.Exists(Environment.CurrentDirectory + "\\Ressources\\lol.xml") == true)
            {
                MessageBox.Show("LOOL");
                StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\Ressources\\lol.xml", Encoding.UTF8);
                str = sr.ReadToEnd();
            }

            if (str.Length > 10)
            {
                using (XmlReader xmlR = XmlReader.Create(new StringReader(str)))
                    this.playListDocument.Load(Environment.CurrentDirectory + "\\Ressources\\lol.xml");
            }
            else
            {
                XmlNode root = this.playListDocument.CreateElement("PlayLists");
                this.playListDocument.AppendChild(root);
            }
            this.loadBackgroundOfAPicture(Environment.CurrentDirectory + "\\Ressources\\stop.png", this.stop);
            this.loadBackgroundOfAPicture(Environment.CurrentDirectory + "\\Ressources\\pause.png", this.pause);
            this.loadBackgroundOfAPicture(Environment.CurrentDirectory + "\\Ressources\\play.png", this.play);
        }

        private void loadBackgroundOfAPicture(string fileName, Button buttonToSet)
        {
            MessageBox.Show(fileName);
            Uri resourceUri = new Uri(fileName, UriKind.Absolute);
            MessageBox.Show("Step 2");
            try
            {
                BitmapImage bm = new BitmapImage();
                bm.BeginInit();
                bm.UriSource = new Uri(fileName);
                bm.EndInit();
                var brush = new ImageBrush();
                brush.ImageSource = bm;
                buttonToSet.Background = brush;
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Source);
            }

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
            this.createMenuItemToContextMenu(contextMenu, "Create Playlist");
            ((MenuItem)contextMenu.Items.GetItemAt(0)).Click += new RoutedEventHandler(this.create_playlist);
            contextMenu.IsOpen = true;
            contextMenu.Visibility = System.Windows.Visibility.Visible;
        }

        private void create_playlist(object sender, EventArgs e)
        {
            if (this.playlistExists("NewPlayList") == true)
                MessageBox.Show("There is arleady a playlist with the \"NewPlayList\" name");
            else
            {
                if (System.IO.File.Exists(Environment.CurrentDirectory + "\\Ressources\\lol.xml") == false)
                {
                    FileStream newFile = System.IO.File.Create(Environment.CurrentDirectory + "\\Ressources\\lol.xml");
                    newFile.Close();
                }
                if (System.IO.File.Exists(Environment.CurrentDirectory + "\\Ressources\\lol.xml") == true)
                {
                    this.fillXmlPlaylistReader();
                    this.addPlaylist();
                    this.fillPlayListFile();
                    TreeViewItem tvi = new TreeViewItem();
                    tvi.Header = "NewPlayList";
                    this.libList.Items.Add(tvi);
                }
            }
        }

        private void addPlaylist()
        {
            XmlNode n1 = this.playListDocument.CreateElement("NewPlayList");
            this.playListDocument.ChildNodes.Item(0).AppendChild(n1);
        }

        private void fillXmlPlaylistReader()
        {
            if (System.IO.File.Exists(Environment.CurrentDirectory + "\\Ressources\\lol.xml") == false)
                return;
            StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\Ressources\\lol.xml", Encoding.UTF8);
            string str = sr.ReadToEnd();
            StringBuilder sb = new StringBuilder();

            if (str.Length > 10)
            {
                using (XmlReader xmlR = XmlReader.Create(new StringReader(str)))
                {
                    this.playListDocument.Load(Environment.CurrentDirectory + "\\Ressources\\lol.xml");
                }
            }
            sr.Close();
        }

        private void fillPlayListFile()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            using (XmlWriter textWriter = XmlWriter.Create(sb, settings))
                this.writeOnFileWithXmlDocument(this.playListDocument, textWriter);
            using (StreamWriter streamWriter = new StreamWriter(Environment.CurrentDirectory + "\\Ressources\\lol.xml", false))
            {
                string strToAdd = sb.ToString();
                strToAdd = strToAdd.Replace("utf-16", "utf-8");
                MessageBox.Show(strToAdd);
                streamWriter.WriteLine(strToAdd);
                streamWriter.Close();
            }
        }

        private void writeOnFileWithXmlDocument(XmlDocument xmlDocument, XmlWriter xmlW)
        {
            foreach (XmlNode nodeToUse in xmlDocument.ChildNodes)
                this.writeChildNodes(xmlDocument, nodeToUse, xmlW);
        }

        private void writeChildNodes(XmlDocument xmlDocument, XmlNode xmlNode, XmlWriter xmlW)
        {
            xmlW.WriteStartElement(xmlNode.Name);
            // Writes attributes of a node
            if (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
                foreach (XmlAttribute xmlA in xmlNode.Attributes)
                    xmlW.WriteElementString(xmlA.Name, xmlA.Value);
            // Recursivity : allows to go deeper in the arborescence of nodes and do the same operation
            if (xmlNode.HasChildNodes == true)
                foreach (XmlNode nodeToUse in xmlNode.ChildNodes)
                    this.writeChildNodes(xmlDocument, nodeToUse, xmlW);
            xmlW.WriteEndElement();
        }

        private List<String> retElement(int option, string title)
        {
            List<String> listToRet = new List<String>();


            return (listToRet);
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
            if (System.IO.File.Exists(Environment.CurrentDirectory + "\\Ressources\\lol.xml") == false)
                System.IO.File.Create(Environment.CurrentDirectory + "\\Ressources\\lol.xml");
            if (System.IO.File.Exists(Environment.CurrentDirectory + "\\Ressources\\lol.xml") == true)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.AddExtension = true;
                fileDialog.DefaultExt = ".";
                fileDialog.CheckPathExists = true;
                fileDialog.ShowDialog();

                TreeViewItem item = new TreeViewItem();

                item.Header = fileDialog.SafeFileName;
                ((TreeViewItem)this.libList.SelectedItem).Items.Add(item);

                System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(Environment.CurrentDirectory + "\\Ressources\\lol.xml", true);
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
            this.nameEntering.setNames("Enter your playlist name", "NewPlayList", "Change your Playlist Name !", this, 0);
            if (this.nameEntering.Visibility == System.Windows.Visibility.Collapsed
                || this.nameEntering.Visibility == System.Windows.Visibility.Hidden)
                this.nameEntering.Visibility = System.Windows.Visibility.Visible;
        }

        private void rename_media(object sender, EventArgs e)
        {
            this.nameEntering.setNames("Enter your media name", "NewMedia", "Change your Media Name !", this, 1);
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

        public void renaming_media(object sender, EventArgs e)
        {
            XmlNode xmlNode = this.playListDocument.GetElementById((string)((TreeViewItem)this.libList.SelectedItem).Header);
            ((TreeViewItem)this.libList.SelectedItem).Header = this.nameEntering.editToUse.Text;
            this.nameEntering.Visibility = System.Windows.Visibility.Hidden;
            xmlNode.InnerXml = xmlNode.InnerXml.Replace("OldName>", "NewName>");
        }

        public void renaming_playlist(object sender, EventArgs e)
        {
            XmlNodeList xmlNode = this.playListDocument.GetElementsByTagName((string)((TreeViewItem)this.libList.SelectedItem).Header);
            ((TreeViewItem)this.libList.SelectedItem).Header = this.nameEntering.editToUse.Text;
            this.nameEntering.Visibility = System.Windows.Visibility.Hidden;
            MessageBox.Show(xmlNode.Count.ToString());
            xmlNode.Item(0).ParentNode.InnerXml = xmlNode.Item(0).ParentNode.InnerXml.Replace(xmlNode.Item(0).Name, (string)((TreeViewItem)this.libList.SelectedItem).Header);
            this.fillPlayListFile();
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
