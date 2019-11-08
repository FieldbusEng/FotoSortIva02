using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoSortIva02.Model
{
    public class MethodsFromStartButton
    {

        // Method to calculate the size of all files
        public string GetSizeOftheFiles(string[] _filesNames)
        {
            if (_filesNames != null)
            {
                int count = 0;

                foreach (string bb in _filesNames)
                {
                    long length = new System.IO.FileInfo(bb).Length;
                    count = count + ((int)length);
                }
                // get count in megabytes( / 1.000.0000) and round it up 
                count = (int)Math.Ceiling((double)count / (double)1000000);
                return count.ToString();
            }
            else
            {
                return "empty";
            }


        }


        // method to return all needed files in folder

        public string[] GetShortFilesFrom(String searchFolder, String[] filters, bool isRecursive)
        {
            List<String> filesFound = new List<String>();
            //Variant 1 search only in choosen folder
            //var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            // Variant 1 -------------------- end

            //Variant 2 search in all subfolders
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.AllDirectories;
            // Variant 2 -------------------- end

            foreach (var filter in filters)
            {
                //Variant 1 this getting the full name of the files with the pathes
                //filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
                // Variant 1 -------------------- end

                //Variant 2: this getting only the name of the file itself without pathes
                string[] filename = System.IO.Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption);
                List<string> intermediate = new List<string>();
                foreach (var str in filename)
                {

                    intermediate.Add(Path.GetFileName(str));
                }
                filesFound.AddRange(intermediate);
                // Variant 2 -------------------- end
            }
            return filesFound.ToArray();
        }

        public string[] GetLongFilesFrom(String searchFolder, String[] filters, bool isRecursive)
        {
            List<String> filesFound = new List<String>();
            //Variant 1 search only in choosen folder
            //var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            // Variant 1 -------------------- end

            //Variant 2 search in all subfolders
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.AllDirectories;
            // Variant 2 -------------------- end

            foreach (var filter in filters)
            {
                //Variant 1 this getting the full name of the files with the pathes
                filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
                // Variant 1 -------------------- end
            }
            return filesFound.ToArray();
        }

    }
}
