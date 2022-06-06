using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FotoSortIva02.Model
{
    class SerialisationProcess_Binary
    {
        public void DoSerialisationB(Dictionary<string, string> _inMovePicDictionary)
        {
            // Serialisation -xml serialisation is not possible for dictionary - thats why i use SerialisationDictionary[] for serialisation
            SerialisationDictionary[] arrayClassDict = new SerialisationDictionary[_inMovePicDictionary.Keys.Count()];
            int ii = 0;
            foreach (KeyValuePair<string, string> item in _inMovePicDictionary)
            {
                SerialisationDictionary temp = new SerialisationDictionary();
                temp.methodSet(item);
                arrayClassDict[ii] = temp;
                ii++;
            }

            BinaryFormatter formater = new BinaryFormatter();
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("secretData.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    formater.Serialize(fs, arrayClassDict);
                }
                catch (Exception e)
                {
                    string messageToWriteFailed = "Serialisation Exception happen" + e.ToString();
                    LoggingTxtIva.GetInstance(messageToWriteFailed);

                }

            }
        }

        public Dictionary<string, string> DoDeserialisationB()
        {
            int numberItems = 0;
            Dictionary<string, string> readyDict = new Dictionary<string, string>();

            BinaryFormatter formater = new BinaryFormatter();
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("secretData.xml", FileMode.OpenOrCreate))
            {
                try
                {

                    SerialisationDictionary[] arrayClassDict1 = (SerialisationDictionary[])formater.Deserialize(fs);
                    numberItems = arrayClassDict1.Length;
                    foreach (var ittem in arrayClassDict1)
                    {
                        readyDict.Add(ittem.keyy, ittem.valuee);
                    }
                }
                catch (Exception e)
                {
                    string messageToWriteFailed = "DE serialisation Exception happen" + e.ToString();
                    LoggingTxtIva.GetInstance(messageToWriteFailed);

                }

            }
            string message = "DEserialisation process finished";
            LoggingTxtIva.GetInstance(message);
            return readyDict;

        }

    }
}
