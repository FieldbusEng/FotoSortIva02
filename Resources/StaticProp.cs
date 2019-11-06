using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoSortIva02.Resources
{
    public static class StaticProp
    {
        // this is the file path of the assembly
        public static string FilePath = String.Format("LOOGGER.txt");

        // string where i store the path of the folder where all creation will be done
        public static string CreateFolderPath = "empty";
        // string where i store the path of the folder where we scan for jpg files 
        public static string ScanningFolderPath = "empty";

        // to show text in text boxes
        public static string initTextBoxScanFolder = "Empty Scan";
        public static string initTextBoxNewFolder = "Empty New folder";
        public static string initTextBoxStatus = "Preparation";
        public static string initTextBoxInfo = " All rights Reserved 2019." +
            " Version 1.0.4."+
            " Free Software."+
            " Contact information: fieldbusff02@gmail.com";

        // ------------------------

        public static string January = "January";
        public static string February = "February";
        public static string March = "March";
        public static string April = "April";
        public static string May = "May";
        public static string June = "June";
        public static string July = "July";
        public static string August = "August";
        public static string September = "September";
        public static string October = "October";
        public static string November = "November";
        public static string December = "December";

        public static System.Collections.Generic.Dictionary<int, string> Monthes = new System.Collections.Generic.Dictionary<int, string>
        {
            {1, January },{2, February }, {3, March },{4, April },{5, May },{6, June },{7, July },{8, August },{9, September },
            {10, October },{11, November },{12, December }

        };


        // Static bool for CheckBoxDelete
        public static bool PropCheckBoxDelete = false;
        // Static bool for CheckBoxVideoSeparateFolder
        public static bool CheckBoxVideoSeparateFolder = false;
    }

       
}
