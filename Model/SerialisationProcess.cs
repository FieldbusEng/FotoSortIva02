using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Xml.Serialization;

namespace FotoSortIva02.Model
{
    // this serialisation xml doesnt work - to delete (because i use binary serialisation)
    
    public class SerialisationProcess
    {
        public void DoSerialisation(Dictionary<string, string> _inMovePicDictionary)
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

            XmlSerializer serializer = new XmlSerializer(typeof(SerialisationDictionary[]));
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("secretData.xml", FileMode.OpenOrCreate))
            {

                try
                {
                    serializer.Serialize(fs, arrayClassDict);

                }
                catch (Exception e)
                {
                    string messageToWriteFailed = "Serialisation Exception happen" + e.ToString();
                    LoggingTxtIva.GetInstance(messageToWriteFailed);

                }

            }
        }

        public Dictionary<string, string> DoDeserialisation()
        {
            int numberItems = 0;
            Dictionary<string, string> readyDict = new Dictionary<string, string>();

            XmlSerializer serzer = new XmlSerializer(typeof(SerialisationDictionary[]));
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("secretData.xml", FileMode.OpenOrCreate))
            {

                try
                {
                    
                    SerialisationDictionary[] arrayClassDict1 = (SerialisationDictionary[])serzer.Deserialize(fs);
                    numberItems = arrayClassDict1.Length;
                    foreach (var ittem in arrayClassDict1)
                    {
                        readyDict.Add(ittem.keyy,ittem.valuee);
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
