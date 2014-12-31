using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace MyWMPv2.Model
{
    class PlaylistManager
    {
        private readonly String _playlistsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyWMPv2\\Playlists.xml";
        private PlaylistManager _me;

        public PlaylistManager()
        {
            _me = this;
        }

        private String ReadFile()
        {
            CheckFileExist();
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(this._playlistsPath))
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
                doc.LoadXml(this.ReadFile());
                XmlNode elemPath = doc.CreateNode(XmlNodeType.Element, "Path", doc.DocumentElement.NamespaceURI);
                elemPath.InnerText = path;
                XmlNode elemName = doc.CreateNode(XmlNodeType.Attribute, "name", doc.DocumentElement.NamespaceURI);
                elemName.Value = name;
                XmlNode elemPlaylist = doc.CreateNode(XmlNodeType.Element, "Playlist", doc.DocumentElement.NamespaceURI);
                elemPlaylist.AppendChild(elemPath);
                elemPlaylist.Attributes.SetNamedItem(elemName);
                XmlElement root = doc.DocumentElement;
                root.AppendChild(elemPlaylist);
                using (StreamWriter w = new StreamWriter(this._playlistsPath, false, Encoding.UTF8))
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
                doc.LoadXml(this.ReadFile());
                XmlNode node = doc.SelectSingleNode("/Playlists/Playlist[@name='" + name + "'][Path='" + path + "']");
                Console.WriteLine("Deleting media : " + node.InnerXml);
                node.ParentNode.RemoveChild(node);
                doc.Save(this._playlistsPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error xml :" + e);
                MessageBox.Show(e.Message, "Error playlist", MessageBoxButton.OK);
            }
        }

        public void RefreshPlaylists(TreeView treePlaylist)
        {
            CheckFileExist();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(this._playlistsPath);
                XmlNodeList nodes = xmlDoc.DocumentElement.SelectNodes("Playlist");
                List<MyMedia> list = new List<MyMedia>();
                foreach (XmlNode node in nodes)
                {
                    MyMedia elem = new MyMedia(ref _me);
                    elem.Playlist = node.Attributes["name"].Value;
                    elem.Path = node.SelectSingleNode("Path").InnerText.Replace("%20", " ").Replace("/", "\\");
                    elem.Name = Path.GetFileNameWithoutExtension(elem.Path).Replace("%20", " ");
                    elem.PlaylistPath = elem.Playlist + "<my1337haxorseparator>" + elem.Path;
                    list.Add(elem);
                }
                LinkListToTreeView(treePlaylist, list);
            }
            catch (XmlException)
            {
                File.Delete(this._playlistsPath);
                this.RefreshPlaylists(treePlaylist);
            }
        }

        private static void LinkListToTreeView(TreeView treePlaylist, List<MyMedia> list)
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
                    playlists.Add(new MyPlaylist() { Name = elem.Playlist, Medias = new List<MyMedia>() });
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
            if (File.Exists(this._playlistsPath))
                return;
            new XDocument(new XElement("Playlists")).Save(this._playlistsPath);
        }
    }
}
