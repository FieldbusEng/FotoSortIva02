using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FotoSortIva02.Model
{
    public class CopyMethods
    {

        public void Copy_FileNameExistsMethod(string _fileToCopy, string _destinationDirectory)
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(_fileToCopy);
            string extension = Path.GetExtension(_fileToCopy);
            string path = Path.GetDirectoryName(_fileToCopy);
            string newFullPath = _destinationDirectory+fileNameOnly+extension;
            string newFullName = _fileToCopy;

            while (File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                //newFullPath = Path.Combine(path, tempFileName + extension);
                newFullName = tempFileName + extension;
                newFullPath = _destinationDirectory+newFullName;
            }

            // Copy 
            File.Copy(_fileToCopy, newFullPath);
            Thread.Sleep(10);
        }

        // needed in case process cannot access the file  because it is being used by another process
        private const int NumberOfRetries = 3;
        private const int DelayOnRetry = 1000;

        public void Move_FileNameExistsMethod(string _fileToCopy, string _destinationDirectory)
        {
            for (int i = 1; i <= NumberOfRetries; i++)
            {
                try
                {
                    int count = 1;
                    string fileNameOnly = Path.GetFileNameWithoutExtension(_fileToCopy);
                    string extension = Path.GetExtension(_fileToCopy);
                    string path = Path.GetDirectoryName(_fileToCopy);
                    string newFullPath = _destinationDirectory + fileNameOnly + extension;
                    string newFullName = _fileToCopy;

                    while (File.Exists(newFullPath))
                    {
                        string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                        //newFullPath = Path.Combine(path, tempFileName + extension);
                        newFullName = tempFileName + extension;
                        newFullPath = _destinationDirectory + newFullName;
                    }

                    // Move
                    File.Move(_fileToCopy, newFullPath);
                    Thread.Sleep(10);
                    break;
                }
                catch (IOException e) when (i <= NumberOfRetries)
                {
                    Thread.Sleep(DelayOnRetry);
                }
            }
            
        }


    }
}
