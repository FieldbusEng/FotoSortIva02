using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoSortIva02.Model
{
    [Serializable]
    public class Pathes
    {
        public string InitialFolderPath { get; set; }

        public Pathes(string initialPath)
        {
            InitialFolderPath = initialPath;
        }

    }
}
