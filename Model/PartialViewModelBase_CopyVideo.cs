using FotoSortIva02.Model;
using FotoSortIva02.Resources;
using System;
using System.IO;
using System.Threading;

namespace FotoSortIva02.ViewModel
{
    partial class ViewModelBase
    {
        enum ActionCopy
        {
          Move,
          Copy
        }
        // Method to Move or Copy Files to the folder taking into account that such a file exists or not
        void Copy_FileNameExistsMethod(string _fileToCopy, string _destinationDirectory, ActionCopy actionCopy)
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(_fileToCopy);
            string extension = Path.GetExtension(_fileToCopy);
            string path = Path.GetDirectoryName(_fileToCopy);
            string newFullPath = _destinationDirectory + fileNameOnly + extension;
            string newFullName = _fileToCopy;

            if (File.Exists(newFullPath) & StaticProp.PropCheckBoxCopyOnlyNew)
            {
                return;
            }
            else if (File.Exists(newFullPath) & !StaticProp.PropCheckBoxCopyOnlyNew)
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                //newFullPath = Path.Combine(path, tempFileName + extension);
                newFullName = tempFileName + extension;
                newFullPath = _destinationDirectory + newFullName;
            }

            if (actionCopy == ActionCopy.Move)
            {
                // Move 
                File.Move(_fileToCopy, newFullPath);
                Thread.Sleep(10);
            }
            else
            {   
                // Copy 
                File.Copy(_fileToCopy, newFullPath);
                Thread.Sleep(2);
            }
        }


        public void MethodCopyVideoFiles(string[] filesVideoCollected)
        {
            if (filesVideoCollected.Length < 1) { return; }

            // making extended Pathes
            string extendedFolderForVideo = StaticProp.CreateFolderPath + string.Format("\\{0}", "Video_Files");

            string fileToCopy;
            string destinationDirectory;

            foreach (string item in filesVideoCollected)
            {
                // to increase ProgressStatusBarValue
                ProgressBarStatusValue++;
                // Copy the file
                fileToCopy = item;
                try
                {
                    // check if such a folder exist
                    if (Directory.Exists(extendedFolderForVideo))
                    {
                        destinationDirectory = extendedFolderForVideo + "\\";
                    }
                    else
                    {
                        Directory.CreateDirectory(extendedFolderForVideo);
                        destinationDirectory = extendedFolderForVideo + "\\";
                    }

                    // in case check box Delete files after copy is true
                    if (StaticProp.PropCheckBoxDelete)
                    {
                        // method to movefiles but also to check if such a file already exist
                        Copy_FileNameExistsMethod(fileToCopy, destinationDirectory, ActionCopy.Move);
                    }
                    else
                    {
                        Copy_FileNameExistsMethod(fileToCopy, destinationDirectory, ActionCopy.Copy);
                    }
                }
                catch (Exception e)
                {
                    string messageToWriteFailed = "Exception happen" + e.ToString();
                    LoggingTxtIva.GetInstance(messageToWriteFailed);

                }

            }

        }

    }
}
