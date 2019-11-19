using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FotoSortIva02.Model
{
    [Serializable]
    public class SerialisationDictionary
    {
        // Dictionary can not be serialized
        public string keyy;
        public string valuee;


        public SerialisationDictionary()
        {}

        public void methodSet(KeyValuePair<string, string> income)
        {
            keyy = income.Key;
            valuee = income.Value;
        }
        
    }
}
