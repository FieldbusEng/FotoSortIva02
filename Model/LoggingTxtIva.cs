using FotoSortIva02.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoSortIva02.Model
{
    class LoggingTxtIva
    {
        #region Singleton Realisation
        // this class will be realized as Singleton, Because it will be used same in different designes of Windowses "MainWindow.xaml" <> "Main_Grid_Style.xaml"
        private static LoggingTxtIva _instance = null;

        private static Object _mutex = new Object();

        public static LoggingTxtIva GetInstance(string arg1)
        {
            if (_instance == null)
            {
                lock (_mutex) // now I can claim some form of thread safety...
                {
                    if (_instance == null)
                    {
                        _instance = new LoggingTxtIva(arg1);
                    }
                }
            }

            return _instance;
        }
        #endregion

        string filePath = StaticProp.FilePath;
        public LoggingTxtIva(string messagetoWrite)
        {

            //Logger
            // read all lines from txt file and put it to List<string>
            List<string> lines = File.ReadAllLines(filePath).ToList();
            // Add time
            lines.Add(DateTime.UtcNow.ToString());
            // message
            lines.Add(messagetoWrite);
            File.WriteAllLines(filePath, lines);
        }
        public LoggingTxtIva(List<string> messagetoWrite)
        {
            //Logger
            // read all lines from txt file and put it to List<string>
            List<string> lines = File.ReadAllLines(filePath).ToList();
            lines = messagetoWrite;
            // Add time
            lines.Add(DateTime.UtcNow.ToString());
            File.WriteAllLines(filePath, lines);
        }
    }
}
