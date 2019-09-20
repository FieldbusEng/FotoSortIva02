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
