using System;
using System.IO;
using System.Text;
using System.Xml;

namespace WMP
{
    class Playlist
    {
        private readonly String Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyWMP\\Playlists.txt";

        public void AddElem(String name, String path)
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyWMP");
            StringBuilder sbuilder = new StringBuilder();
            using (StringWriter sw = new StringWriter(sbuilder))
            {
                using (XmlTextWriter w = new XmlTextWriter(sw))
                {
                    w.WriteStartElement("Playlist");
                    w.WriteElementString("Name", name);
                    w.WriteElementString("Path", path);
                    w.WriteEndElement();
                }
            }
            using (StreamWriter w = new StreamWriter(Path, true, Encoding.UTF8))
            {
                w.WriteLine(sbuilder.ToString());
            }
        }
    }
}
