using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using MyWMPv2.Utilities;

namespace MyWMPv2.Model
{
    class PlaylistManager
    {
        #region Private member variables
        private readonly String _playlistsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyWMPv2\\Playlists.xml";
        private PlaylistManager _me;
        private List<String> _currentPlaylist;
        private readonly Random _random;
        #endregion Private member variables

        public PlaylistManager()
        {
            _me = this;
            _currentPlaylist = new List<string>();
            _random = new Random();
        }

        #region Public member variables
        public List<String> CurrentPlaylist
        {
            get { return _currentPlaylist; }
            set { _currentPlaylist = value; }
        }
        #endregion Public member variables

        private String ReadFile()
        {
            CheckFileExist();
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(_playlistsPath))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            return sb.ToString();
        }

        public void AddElem(String name, String path)
        {
            CheckFileExist();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(ReadFile());
                XmlNode elemPath = doc.CreateNode(XmlNodeType.Element, "Path", doc.DocumentElement.NamespaceURI);
                elemPath.InnerText = path;
                XmlNode elemName = doc.CreateNode(XmlNodeType.Attribute, "name", doc.DocumentElement.NamespaceURI);
                elemName.Value = name;
                XmlNode elemPlaylist = doc.CreateNode(XmlNodeType.Element, "Playlist", doc.DocumentElement.NamespaceURI);
                elemPlaylist.AppendChild(elemPath);
                elemPlaylist.Attributes.SetNamedItem(elemName);
                XmlElement root = doc.DocumentElement;
                root.AppendChild(elemPlaylist);
                using (StreamWriter w = new StreamWriter(_playlistsPath, false, Encoding.UTF8))
                {
                    w.WriteLine(doc.OuterXml);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error xml :" + e);
                MessageBox.Show(e.Message, "Error playlist", MessageBoxButton.OK);
            }
        }

        public void DeleteElem(String name, String path)
        {
            CheckFileExist();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(ReadFile());
                //xpath
                XmlNode node = doc.SelectSingleNode("/Playlists/Playlist[@name='" + name + "'][Path=\"" + path + "\"]");
                Console.WriteLine("Deleting media : " + node.InnerXml);
                node.ParentNode.RemoveChild(node);
                doc.Save(_playlistsPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error xml :" + e);
                MessageBox.Show(e.Message, "Error playlist", MessageBoxButton.OK);
            }
        }

        public void RefreshPlaylists(TreeView treePlaylist, String fgList)
        {
            CheckFileExist();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_playlistsPath);
                XmlNodeList nodes = xmlDoc.DocumentElement.SelectNodes("Playlist");
                List<MyMedia> list = new List<MyMedia>();
                foreach (XmlNode node in nodes)
                {
                    MyMedia elem = new MyMedia(ref _me);
                    elem.Playlist = node.Attributes["name"].Value;
                    elem.Path = node.SelectSingleNode("Path").InnerText.Replace("%20", " ").Replace("/", "\\");
                    elem.Filename = elem.Path.Replace("%20", " ");
                    elem.Name = Path.GetFileNameWithoutExtension(elem.Path).Replace("%20", " ");
                    elem.PlaylistPath = elem.Playlist + "<my1337haxorseparator>" + elem.Path;
                    elem.FgList = Converter.StringToColor(fgList);
                    list.Add(elem);
                }
                LinkListToTreeView(treePlaylist, list, fgList);
            }
            catch (XmlException)
            {
                File.Delete(_playlistsPath);
                this.RefreshPlaylists(treePlaylist, fgList);
            }
        }

        public void SetCurrentPlaylist(String playlistName)
        {
            CheckFileExist();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_playlistsPath);
                XmlNodeList nodes = xmlDoc.DocumentElement.SelectNodes("Playlist");
                _currentPlaylist.Clear();
                foreach (XmlNode node in nodes)
                {
                    String name = node.Attributes["name"].Value;
                    if (name.Equals(playlistName))
                        _currentPlaylist.Add(node.SelectSingleNode("Path").InnerText.Replace("%20", " ").Replace("/", "\\"));
                }
            }
            catch (XmlException)
            {
                File.Delete(_playlistsPath);
                Console.WriteLine("Error setting current playlist");
            }
        }
        public void RandomCurrentPlaylist()
        {
            List<KeyValuePair<int, String>> tmp = new List<KeyValuePair<int, String>>();
            foreach (var s in _currentPlaylist)
                tmp.Add(new KeyValuePair<int, String>(_random.Next(), s));
            var sorted = from item in tmp
                orderby item.Key
                select item;
            int i = 0;
            foreach (var s in sorted)
            {
                _currentPlaylist[i] = s.Value;
                ++i;
            }
        }
        public void RenamePlaylist(String name, String newName)
        {
            CheckFileExist();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(ReadFile());
                //xpath
                XmlNodeList nodes = doc.SelectNodes("/Playlists/Playlist[@name='" + name + "']");
                foreach (XmlNode node in nodes)
                    node.Attributes["name"].Value = newName;
                doc.Save(_playlistsPath);
            }
            catch (XmlException)
            {
                File.Delete(_playlistsPath);
                Console.WriteLine("Error renaming playlist");
            }
        }
        public void DeletePlaylist(String name)
        {
            CheckFileExist();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(ReadFile());
                //xpath
                XmlNodeList nodes = doc.SelectNodes("/Playlists/Playlist[@name='" + name + "']");
                foreach (XmlNode node in nodes)
                    node.ParentNode.RemoveChild(node);
                doc.Save(_playlistsPath);
            }
            catch (XmlException)
            {
                File.Delete(_playlistsPath);
                Console.WriteLine("Error renaming playlist");
            }
        }

        private static void LinkListToTreeView(TreeView treePlaylist, List<MyMedia> list, String fgList)
        {
            List<MyPlaylist> playlists = new List<MyPlaylist>();
            foreach (MyMedia elem in list)
            {
                bool isPlaylist = false;
                foreach (var playlist in playlists.Where(playlist => playlist.Name == elem.Playlist))
                {
                    isPlaylist = true;
                    playlist.Medias.Add(elem);
                }
                if (!isPlaylist)
                {
                    playlists.Add(new MyPlaylist() { Name = elem.Playlist, Medias = new List<MyMedia>(), FgList = Converter.StringToColor(fgList) });
                    playlists[playlists.Count - 1].Medias.Add(elem);
                }
            }
            treePlaylist.Items.Clear();
            foreach (var playlist in playlists)
                treePlaylist.Items.Add(playlist);
        }

        private void CheckFileExist()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyWMPv2"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyWMPv2");
            if (File.Exists(_playlistsPath))
                return;
            new XDocument(new XElement("Playlists")).Save(_playlistsPath);
        }
    }
}
