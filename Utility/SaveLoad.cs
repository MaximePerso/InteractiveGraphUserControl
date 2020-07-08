using InteractiveGraphUserControl.MVVM;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace InteractiveGraphUserControl.Utility
{
    class SaveLoad
    {
        public SaveLoad()
        {

        }

        public void Save(ObservableCollection<DoliInput> DoliInputCollection)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            fd.DefaultExt = ".xml";
            fd.FileName = "MyConfig.xml";
            if (fd.ShowDialog() == true)
            {
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DoliInput>));
                using (StreamWriter wr = new StreamWriter(fd.FileName))
                {
                    xs.Serialize(wr, DoliInputCollection);
                }
            }
        }

        public ObservableCollection<DoliInput> Load()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            fd.DefaultExt = ".xml";
            fd.FileName = "MyConfig.xml";
            if (fd.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(fd.FileName))
                {
                    Console.WriteLine(fd.FileName);
                    XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DoliInput>));
                    using (StreamReader rd = new StreamReader(fd.FileName))
                    {
                        try
                        {
                            return xs.Deserialize(rd) as ObservableCollection<DoliInput>;
                        }
                        catch
                        {
                            MessageBox.Show("Wrong file");
                            return null;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
