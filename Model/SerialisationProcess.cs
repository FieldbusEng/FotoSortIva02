﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Xml.Serialization;

namespace FotoSortIva02.Model
{
    public class SerialisationProcess
    {
        public void DoSerialisation(Dictionary<string, string> _inMovePicDictionary)
        {
            // Serialisation -xml serialisation is not possible for dictionary - thats why i use SerialisationDictionary[] for serialisation
            SerialisationDictionary serDictClass = new SerialisationDictionary();
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
                    LoggingTxtIva ll2 = new LoggingTxtIva(messageToWriteFailed);

                }

            }
        }

        public Dictionary<string, string> DoDeserialisation()
        {
            int numberItems = 0;
            Dictionary<string, string> readyDict = new Dictionary<string, string>();

            XmlSerializer serializer = new XmlSerializer(typeof(SerialisationDictionary[]));
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("secretData.xml", FileMode.OpenOrCreate))
            {

                try
                {
                  
                    SerialisationDictionary[] arrayClassDict = (SerialisationDictionary[])serializer.Deserialize(fs);
                    numberItems = arrayClassDict.Length;
                    foreach (var ittem in arrayClassDict)
                    {
                        readyDict.Add(ittem.keyy,ittem.valuee);
                    }
                }
                catch (Exception e)
                {
                    string messageToWriteFailed = "DE serialisation Exception happen" + e.ToString();
                    LoggingTxtIva ll2 = new LoggingTxtIva(messageToWriteFailed);

                }

            }

            return readyDict;
            

        }

    }
}
