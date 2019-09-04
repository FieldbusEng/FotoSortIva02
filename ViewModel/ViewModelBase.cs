using ExifLib;
using FotoSortIva02.Model;
using FotoSortIva02.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace FotoSortIva02.ViewModel
{
    public class ViewModelBase : TextBoxesModel
    {
        #region CTOR
        string filePath;
        public ViewModelBase()
        {
            // Delete the log file from the beggining of the Test!
            string FilePath = String.Format("LOOGGER.txt");
            filePath = FilePath;
            // read all lines from txt file and put it to List<string>
            List<string> lines = File.ReadAllLines(filePath).ToList();
            List<string> EmptyList = new List<string> { };
            // make lines to be empty
            lines = EmptyList;
            // write to LOG
            File.WriteAllLines(filePath, lines);

            TextBoxGenShow = "Initial Value";
            TextBoxNewFolder = StaticProp.initTextBoxNewFolder;
            TextBoxScanFolder = StaticProp.initTextBoxScanFolder;

        }
        #endregion

        #region Button StartButtCommand

        private ICommand _startButtCommand;
        public ICommand StartButtCommand
        {
            get
            {
                return _startButtCommand ?? (_startButtCommand = new CommandHandler(() => StartButtAction(), () => CanExecute));
            }
        }

        void StartButtAction()
        {
            // todo change belo line back
            if (StaticProp.ScanningFolderPath != "empty" && StaticProp.CreateFolderPath != "empty")
            //if (StaticProp.ScanningFolderPath != "empty")
            {
                // Change TextBoxStatus
                TextBoxStatus = "Process Started";

                Pathes pathing = new Pathes(StaticProp.ScanningFolderPath);

                String searchFolder = pathing.InitialFolderPath;
                var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
                filesNames = GetLongFilesFrom(searchFolder, filters, false);
                foreach (string item in filesNames)
                {
                    try
                    {
                        using (ExifReader reader = new ExifReader(item))
                        {
                            // Extract the tag data using the ExifTags enumeration
                            DateTime datePictureTaken;
                            if (reader.GetTagValue<DateTime>(ExifTags.DateTimeDigitized, out datePictureTaken))
                            {
                                // Do whatever is required with the extracted information
                                string messageToWriteSuccess = "The picture was taken on  " +datePictureTaken.ToString();


                                //Logger
                                // read all lines from txt file and put it to List<string>
                                List<string> lines = File.ReadAllLines(filePath).ToList();
                                // Add time
                                lines.Add(DateTime.UtcNow.ToString());
                                // message
                                lines.Add(messageToWriteSuccess);

                                File.WriteAllLines(filePath, lines);

                                int intmonthOfPic = (Int32)datePictureTaken.Month;
                                string monthOfPic = "NOMonth";
                                StaticProp.Monthes.TryGetValue(intmonthOfPic, out monthOfPic);
                                string yearOfPic = datePictureTaken.Year.ToString();
                                // making extended Pathes
                                string extendedYearPath = StaticProp.CreateFolderPath + string.Format("\\{0}", yearOfPic);
                                string extendedMonthPath = extendedYearPath + string.Format("\\{0}", monthOfPic);
                                //---------------------


                                // check if such a year exist in the new folder
                                if (File.Exists(extendedYearPath))
                                {
                                    //check if such a month exist in the new folder
                                    if (File.Exists(extendedMonthPath))
                                    {
                                        

                                    }
                                }
                                else
                                {
                                    Directory.CreateDirectory(extendedYearPath);
                                    Directory.CreateDirectory(extendedMonthPath);
                                }



                            }
                        }
                    }
                    catch (Exception e)
                    {
                        string messageToWriteFailed = "Exception happen" + e.ToString();

                        //Logger
                        // read all lines from txt file and put it to List<string>
                        List<string> lines = File.ReadAllLines(filePath).ToList();
                        // Add time
                        lines.Add(DateTime.UtcNow.ToString());
                        // message
                        lines.Add(messageToWriteFailed);

                        File.WriteAllLines(filePath, lines);

                        // here i need to create directory in case Exif Data not exist
                        string extendedNoExif = StaticProp.CreateFolderPath + "\\No_Date";
                        Directory.CreateDirectory(extendedNoExif);
                    }
                }
                


            }
            else
            {
                MessageBox.Show("You have to choose SCAN folder and NEW folder", "Help", MessageBoxButtons.OK);
                // Change TextBoxStatus
                TextBoxStatus = "Folders not determined";

            }
            // Change TextBoxStatus
            TextBoxStatus = "Process Finished!";
        }
        #endregion

        #region Button LogButtCommand

        private ICommand _logButtCommand;
        public ICommand LogButtCommand
        {
            get
            {
                return _logButtCommand ?? (_logButtCommand = new CommandHandler(() => LogButtAction(), () => CanExecute));
            }
        }

        void LogButtAction()
        {
            // create the path
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            //Note that the value in path will be in the form of ..file:\c:\path\to\bin\folder, so before
            //using the path you may need to strip the file:\ off the front.
            path = path.Substring(6);

            Process.Start(path);
        }
        #endregion




        #region Button CreateButtCommand
        private ICommand _createButtCommand;
        public ICommand CreateButtCommand
        {
            get
            {
                return _createButtCommand ?? (_createButtCommand = new CommandHandler(() => CreateButtAction(), () => CanExecute));
            }
        }

        void CreateButtAction()
        {
            //use FolderBrowserDialog instead of FileDialog and get the path from the OK result.

            FolderBrowserDialog browser = new FolderBrowserDialog();
            
            if (browser.ShowDialog() == DialogResult.OK)
            {
                StaticProp.CreateFolderPath = browser.SelectedPath;
                TextBoxNewFolder = StaticProp.CreateFolderPath;
                
            }

            // Change TextBoxStatus
            TextBoxStatus = "Folder New is choosen";
        }
        #endregion

        #region Button ScanButtCommand
        private ICommand _scanButtCommand;
        public ICommand ScanButtCommand
        {
            get
            {
                return _scanButtCommand ?? (_scanButtCommand = new CommandHandler(() => ScanButtAction(), () => CanExecute));
            }
        }
        public bool CanExecute
        {
            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                return true;
            }
        }
     
        string[] filesNames = new string[] { };
        void ScanButtAction()
        {
            // Change TextBoxStatus
            TextBoxStatus = "Scanning Started";

            //use FolderBrowserDialog instead of FileDialog and get the path from the OK result.

            FolderBrowserDialog browser = new FolderBrowserDialog();
            

            if (browser.ShowDialog() == DialogResult.OK)
            {
                StaticProp.ScanningFolderPath = browser.SelectedPath; // prints path
                TextBoxScanFolder = StaticProp.ScanningFolderPath;
            }
            if (StaticProp.ScanningFolderPath != "empty")
            {
                Pathes pathing = new Pathes(StaticProp.ScanningFolderPath);

                String searchFolder = pathing.InitialFolderPath;
                var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
                filesNames = GetShortFilesFrom(searchFolder, filters, false);
                TextBoxFotoCounter=TextBoxFotoCounterMethod(filesNames);
                TextGenShowMethod(string.Join("\r\n", filesNames));
            }
            TextBoxStatus = "Scanning Finished";

        }

        // method to return all needed files in folder

        string[] GetShortFilesFrom(String searchFolder, String[] filters, bool isRecursive)
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

        string[] GetLongFilesFrom(String searchFolder, String[] filters, bool isRecursive)
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


        #endregion
        void TextGenShowMethod(string income)
        {
            TextBoxGenShow = income;
        }
        
        string TextBoxFotoCounterMethod(string[] income)
        {
            int count = 0;

            foreach (var item in income)
            {
                count++;
            }

            return count.ToString();
        }

        #region Button Exit
        private ICommand _exitButtCommand;
        public ICommand ExitButtCommand
        {
            get
            {
                return _exitButtCommand ?? (_exitButtCommand = new CommandHandler(() => ExitButtAction(), () => CanExecute));
            }
        }

        void ExitButtAction()
        {
            // add some safety not possible to close while files not copied...
            App.Current.Shutdown();

        }
        #endregion
    }
}
