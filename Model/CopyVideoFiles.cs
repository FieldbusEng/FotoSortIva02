using FotoSortIva02.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoSortIva02.Model
{
    class CopyVideoFiles
    {

        public CopyVideoFiles(string[] filesVideoCollected)
        {
            //
            if (StaticProp.CheckBoxVideoSeparateFolder == true)
            {

                foreach (string item in filesVideoCollected)
                {
                    ProgressBarStatusValue++;
                    try
                    {

                    }
                    catch
                    {

                    }

                }
            }
            else
            {
                return;
            }

        }
    }
}
