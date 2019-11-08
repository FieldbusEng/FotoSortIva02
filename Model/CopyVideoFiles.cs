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
    class CopyVideoFiles
    {
        // Method to Move Files to the folder taking into account that such a file exists or not
        void Move_FileNameExistsMethod(string _fileToCopy, string _destinationDirectory)
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
            Thread.Sleep(200);
        }

        // Method to Copy Files to the folder taking into account that such a file exists or not
        void Copy_FileNameExistsMethod(string _fileToCopy, string _destinationDirectory)
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

            // Copy 
            File.Copy(_fileToCopy, newFullPath);
            Thread.Sleep(200);
        }

        public CopyVideoFiles(string[] filesVideoCollected)
        {
            // in case check box Video As Well to separate Folder is true
            if (StaticProp.CheckBoxVideoSeparateFolder == true)
            {
                // in case check box Delete files after copy is true
                if (StaticProp.PropCheckBoxDelete)
                {
                    foreach (string item in filesVideoCollected)
                    {

                        try
                        {

                            // making extended Pathes
                            string extendedFolderForVideo = StaticProp.CreateFolderPath + string.Format("\\{0}", "Video_Files");
                            //---------------------
                            // check if such a folder exist
                            if (File.Exists(extendedFolderForVideo))
                            {
                                // Copy the file
                                string fileToCopy = item;
                                string destinationDirectory = extendedFolderForVideo + "\\";

                                // method to movefiles but also to check if such a file already exist
                                Move_FileNameExistsMethod(fileToCopy, destinationDirectory);

                            }
                            else
                            {
                                // Copy the file
                                Directory.CreateDirectory(extendedFolderForVideo);
                                string fileToCopy = item;
                                string destinationDirectory = extendedFolderForVideo + "\\";

                                // method to movefiles but also to check if such a file already exist
                                Move_FileNameExistsMethod(fileToCopy, destinationDirectory);

                                //File.Move(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));
                                //Thread.Sleep(200);

                            }

                        }
                        catch (Exception e)
                        {
                            string messageToWriteFailed = "Exception happen" + e.ToString();
                            LoggingTxtIva ll6 = new LoggingTxtIva(messageToWriteFailed);

                        }

                    }
                }
                else
                {
                    // in case check box Delete files after copy is False
                    foreach (string item in filesVideoCollected)
                    {

                        try
                        {

                            // making extended Pathes
                            string extendedFolderForVideo = StaticProp.CreateFolderPath + string.Format("\\{0}", "Video_Files");
                            //---------------------
                            // check if such a folder exist
                            if (File.Exists(extendedFolderForVideo))
                            {

                                // Copy the file
                                string fileToCopy = item;
                                string destinationDirectory = extendedFolderForVideo + "\\";

                                // method to copy files but also to check if such a file already exist
                                Copy_FileNameExistsMethod(fileToCopy, destinationDirectory);
                                
                                //File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));


                            }
                            else
                            {
                                // Copy the file
                                Directory.CreateDirectory(extendedFolderForVideo);
                                string fileToCopy = item;
                                string destinationDirectory = extendedFolderForVideo + "\\";
                                // method to copy files but also to check if such a file already exist
                                Copy_FileNameExistsMethod(fileToCopy, destinationDirectory);

                                //File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));

                            }

                        }
                        catch (Exception e)
                        {
                            string messageToWriteFailed = "Exception happen" + e.ToString();
                            LoggingTxtIva ll6 = new LoggingTxtIva(messageToWriteFailed);

                        }

                    }

                }


            }
            else
            {
                // in case check box Move Video Files to separate Folder is False
                LoggingTxtIva ll6 = new LoggingTxtIva("check box Move Video Files to separate Folder is False");
            }

        }
    }
}
