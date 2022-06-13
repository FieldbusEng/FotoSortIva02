using FotoSortIva02.Resources;
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

        #region Singleton Realisation
        // this class will be realized as Singleton, Because it will be used same in different designes of Windowses "MainWindow.xaml" <> "Main_Grid_Style.xaml"
        private static CopyMethods _instance;
        public static CopyMethods Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CopyMethods();
                }
                return _instance;
            }
        }

        #endregion


        public void Copy_FileNameExistsMethod(string _fileToCopy, string _destinationDirectory, ActionCopy actionCopy)
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(_fileToCopy);
            string extension = Path.GetExtension(_fileToCopy);
            string path = Path.GetDirectoryName(_fileToCopy);
            string newFullPath = _destinationDirectory+fileNameOnly+extension;
            string newFullName = _fileToCopy;

            // if file exist and need to copy only new
            if (File.Exists(newFullPath) & StaticProp.PropCheckBoxCopyOnlyNew)
            {
                return;
            }
            else if(File.Exists(newFullPath) & !StaticProp.PropCheckBoxCopyOnlyNew) // if file exist but copy anyway
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                //newFullPath = Path.Combine(path, tempFileName + extension);
                newFullName = tempFileName + extension;
                newFullPath = _destinationDirectory + newFullName;
            }

            if (actionCopy == ActionCopy.Copy) // Copy Called
            {
                File.Copy(_fileToCopy, newFullPath);
                Thread.Sleep(2);
            }
            else // Move Called
            {
                Move_Method(_fileToCopy, newFullPath);
                Thread.Sleep(2);
            }
            

        }
        // needed in case process cannot access the file  because it is being used by another process
        private const int NumberOfRetries = 3;
        private const int DelayOnRetry = 200;

        void Move_Method(string _fileToCopy, string _newFullPath)
        {
            for (int i = 1; i <= NumberOfRetries; i++)
            {
                try
                {
                    #region TEST1 here is was experiment to be able to open the file and close it - so no other process access the file !!!
                    try
                    {
                        FileStream fs = System.IO.File.Open(_fileToCopy, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read, System.IO.FileShare.None);
                        fs.Close();
                    }
                    catch (IOException e)
                    {
                        string messageToWriteFailed = "Exception happen" + e.ToString();
                        LoggingTxtIva.GetInstance(messageToWriteFailed);
                    }
                    #endregion

                    // Move
                    File.Move(_fileToCopy, _newFullPath);
                    Thread.Sleep(2);
                    break;
                }
                catch (IOException e) when (i <= NumberOfRetries)
                {
                    string messageToWriteFailed = "Exception happen" + e.ToString();
                    LoggingTxtIva.GetInstance(messageToWriteFailed);

                    Thread.Sleep(DelayOnRetry);
                }
            }
            
        }


    }
}
