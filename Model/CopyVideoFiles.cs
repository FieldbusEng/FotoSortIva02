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

        public CopyVideoFiles(string[] filesVideoCollected)
        {
            // in case check box Move Video Files to separate Folder is true
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
                                File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));
                                Thread.Sleep(200);
                                // Delete original file
                                File.Delete(fileToCopy);

                            }
                            else
                            {
                                // Copy the file
                                Directory.CreateDirectory(extendedFolderForVideo);
                                string fileToCopy = item;
                                string destinationDirectory = extendedFolderForVideo + "\\";
                                File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));
                                Thread.Sleep(200);
                                // Delete original file
                                File.Delete(fileToCopy);
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
                                File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));

                            }
                            else
                            {
                                // Copy the file
                                Directory.CreateDirectory(extendedFolderForVideo);
                                string fileToCopy = item;
                                string destinationDirectory = extendedFolderForVideo + "\\";
                                File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));

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
