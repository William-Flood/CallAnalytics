using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Control
{
    public class SettingsManager
    {
        const string FOLDERNAME = "CallAnalytics";
        const string FILENAME = "Config.xml";
        const string LAST_SEARCHED_FOLDER_ELEM = "lastSearchedFolder";
        public SettingsManager()
        {
            var settingsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FOLDERNAME);
            if(!Directory.Exists(settingsFolder))
            {
                Directory.CreateDirectory(settingsFolder);
            }
            var settingsFile = Path.Combine(settingsFolder, FILENAME);
            if(File.Exists(settingsFile))
            {
                try
                {
                    using(var netReader = XmlReader.Create(settingsFile))
                    {
                        var netDocument = new XmlDocument();
                        netDocument.Load(netReader);
                        var root = netDocument.LastChild;
                        _lastSearchedFolder = root.SelectNodes(LAST_SEARCHED_FOLDER_ELEM)[0].InnerText;
                    }
                } catch(DirectoryNotFoundException ex)
                {
                    throw new Exception("Couldn't load file", ex);
                }
            }
        }

        private String _lastSearchedFolder;
        public String LastSearchedFolder
        {
            get
            {
                return _lastSearchedFolder;
            }
            set
            {
                _lastSearchedFolder = value;
                Save();
            }
        }

        public void Save()
        {
            var saveFile = Path.Combine(new String[] { Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FOLDERNAME, FILENAME});
            try
            {
                using(var netWriter = XmlWriter.Create(saveFile))
                {
                    netWriter.WriteStartElement("setting");
                    netWriter.WriteElementString(LAST_SEARCHED_FOLDER_ELEM, _lastSearchedFolder);
                    netWriter.WriteEndElement();
                    netWriter.Flush();
                }
            } catch (Exception ex)
            {
                throw new Exception("Error saving", ex);
            }
        }
    }
}
