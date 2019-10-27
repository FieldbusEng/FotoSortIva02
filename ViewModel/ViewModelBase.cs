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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace FotoSortIva02.ViewModel
{
    public class ViewModelBase : TextBoxesModel
    {
        #region Singleton Realisation
        // this class will be realized as Singleton, Because it will be used same in different designes of Windowses "MainWindow.xaml" <> "Main_Grid_Style.xaml"
        private static ViewModelBase _instance;
        public static ViewModelBase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ViewModelBase();
                }
                return _instance;
            }
        }

        public string name_singleton { get; set; }
        #endregion


        string[] filesNames = new string[] { };
        string[] filesNamesVideo = new string[] { };

        #region CTOR
        private ViewModelBase()
        {


            // read all lines from txt file and put it to List<string>
            List<string> EmptyList = new List<string> { };
            LoggingTxtIva ll0 = new LoggingTxtIva(EmptyList);


            TextBoxGenShow = "No Pictures choosen";
            TextBoxNewFolder = StaticProp.initTextBoxNewFolder;
            TextBoxScanFolder = StaticProp.initTextBoxScanFolder;

            ProgressBarStatusVisible = Visibility.Hidden;

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

        async void StartButtAction()
        {
            if (StaticProp.PropCheckBoxDelete)
            {
                #region In case of CheckBox is True
                // todo change below line back
                if (StaticProp.ScanningFolderPath != "empty" && StaticProp.CreateFolderPath != "empty")
                //if (StaticProp.ScanningFolderPath != "empty")
                {

                    await Task.Run(() =>
                    {
                    // Change TextBoxStatus
                    TextBoxStatus = "Process Started";
                    // Progress bar visible
                    ProgressBarStatusVisible = Visibility.Visible;

                    Pathes pathing = new Pathes(StaticProp.ScanningFolderPath);

                    String searchFolder = pathing.InitialFolderPath;
                    var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
                        filesNames = GetLongFilesFrom(searchFolder, filters, false);

                        // i work with video files separately
                        // make filter for video files
                    var filtersVideo = new String[] { "mp4", "avi", "mpg", "mpeg", "m2v", "mpg", "mp2", "mpeg", "mpe", "mpv", "mp4", "m4p", "m4v", "amv", "rmvb", "rm", "yuv", "wmv", "mov", "qt", "mng", "gifv", "gif", "ogv", "ogg", "vob", "flv", "mkv" };
                        filesNamesVideo = GetLongFilesFrom(searchFolder, filtersVideo, false);
                        CopyVideoFiles copyvideoInst = new CopyVideoFiles(filesNamesVideo);

                        foreach (string item in filesNames)
                        {
                            ProgressBarStatusValue++;
                            try
                            {
                                
                                using (ExifReader reader = new ExifReader(item))
                                {
                                    // Extract the tag data using the ExifTags enumeration
                                    DateTime datePictureTaken;
                                    if (reader.GetTagValue<DateTime>(ExifTags.DateTimeDigitized, out datePictureTaken))
                                    {
                                        // Do whatever is required with the extracted information
                                        string messageToWriteSuccess = "The picture was taken on  " + datePictureTaken.ToString();
                                        LoggingTxtIva ll1 = new LoggingTxtIva(messageToWriteSuccess);
                                                                                
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
                                                // Copy the file
                                                string fileToCopy = item;
                                                string destinationDirectory = extendedMonthPath + "\\";
                                                File.Move(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));
                                                
                                            }
                                            else
                                            {
                                                Directory.CreateDirectory(extendedMonthPath);
                                                string fileToCopy = item;
                                                string destinationDirectory = extendedMonthPath + "\\";
                                                File.Move(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));
                                                
                                            }

                                        }
                                        else
                                        {
                                            // Copy the file
                                            Directory.CreateDirectory(extendedYearPath);
                                            Directory.CreateDirectory(extendedMonthPath);
                                            string fileToCopy = item;
                                            string destinationDirectory = extendedMonthPath + "\\";
                                            File.Move(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));
                                            
                                        }

                                        Thread.Sleep(10);

                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                string messageToWriteFailed = "Exception happen" + e.ToString();
                                LoggingTxtIva ll2 = new LoggingTxtIva(messageToWriteFailed);
                                
                                // here i need to create directory in case Exif Data not exist
                                string extendedNoExif = StaticProp.CreateFolderPath + "\\No_Date";
                                Directory.CreateDirectory(extendedNoExif);
                                // Copy the file
                                string fileToCopy = item;
                                string destinationDirectory = extendedNoExif + "\\";
                                File.Move(fileToCopy, destinationDirectory + Path.GetFileName("\\" + fileToCopy));
                            }
                        }

                    });

                    // Change TextBoxStatus
                   TextBoxStatus = "Process Finished!";

                }
                else
                {
                    //MessageBox.Show("You have to choose SCAN folder and NEW folder", "Help", MessageBoxButtons.OK);
                    // Change TextBoxStatus
                    TextBoxStatus = "Folders not determined";

                }

                #endregion
            }
            else
            {
                #region In case of CheckBox Delete is False
                // todo change belo line back
                if (StaticProp.ScanningFolderPath != "empty" && StaticProp.CreateFolderPath != "empty")
                //if (StaticProp.ScanningFolderPath != "empty")
                {

                    await Task.Run(() =>
                    {
                        // Change TextBoxStatus
                        TextBoxStatus = "Process Started";
                        // Progress bar visible
                        ProgressBarStatusVisible = Visibility.Visible;

                        Pathes pathing = new Pathes(StaticProp.ScanningFolderPath);

                        String searchFolder = pathing.InitialFolderPath;
                        var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
                        filesNames = GetLongFilesFrom(searchFolder, filters, false);

                        // i work with video files separately
                        // make filter for video files
                        var filtersVideo = new String[] { "mp4", "avi", "mpg", "mpeg", "m2v", "mpg", "mp2", "mpeg", "mpe", "mpv", "mp4", "m4p", "m4v", "amv", "rmvb", "rm", "yuv", "wmv", "mov", "qt", "mng", "gifv", "gif", "ogv", "ogg", "vob", "flv", "mkv" };
                        filesNamesVideo = GetLongFilesFrom(searchFolder, filtersVideo, false);
                        CopyVideoFiles copyvideoInst = new CopyVideoFiles(filesNamesVideo);

                        foreach (string item in filesNames)
                        {
                            ProgressBarStatusValue++;
                            try
                            {
                                using (ExifReader reader = new ExifReader(item))
                                {
                                    // Extract the tag data using the ExifTags enumeration
                                    DateTime datePictureTaken;
                                    if (reader.GetTagValue<DateTime>(ExifTags.DateTimeDigitized, out datePictureTaken))
                                    {
                                        // Do whatever is required with the extracted information
                                        string messageToWriteSuccess = "The picture was taken on  " + datePictureTaken.ToString();
                                        LoggingTxtIva ll4 = new LoggingTxtIva(messageToWriteSuccess);
                                        
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
                                                // Copy the file
                                                string fileToCopy = item;
                                                string destinationDirectory = extendedMonthPath + "\\";
                                                File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));

                                            }
                                            else
                                            {
                                                Directory.CreateDirectory(extendedMonthPath);
                                                string fileToCopy = item;
                                                string destinationDirectory = extendedMonthPath + "\\";
                                                File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));
                                            }
                                        }
                                        else
                                        {
                                            // Copy the file
                                            Directory.CreateDirectory(extendedYearPath);
                                            Directory.CreateDirectory(extendedMonthPath);
                                            string fileToCopy = item;
                                            string destinationDirectory = extendedMonthPath + "\\";
                                            File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));
                                        }


                                        Thread.Sleep(10);

                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                string messageToWriteFailed = "Exception happen" + e.ToString();
                                LoggingTxtIva ll5 = new LoggingTxtIva(messageToWriteFailed);
                               
                                // here i need to create directory in case Exif Data not exist
                                string extendedNoExif = StaticProp.CreateFolderPath + "\\No_Date";
                                Directory.CreateDirectory(extendedNoExif);
                                // Copy the file
                                string fileToCopy = item;
                                string destinationDirectory = extendedNoExif + "\\";

                                File.Copy(fileToCopy, destinationDirectory + Path.GetFileName("\\" + fileToCopy));

                            }
                        }

                    });
                    // Change TextBoxStatus
                    TextBoxStatus = "Process Finished!";



                }
                else
                {
                    //MessageBox.Show("You have to choose SCAN folder and NEW folder", "Help", MessageBoxButtons.OK);
                    // Change TextBoxStatus
                    TextBoxStatus = "Folders not determined";

                }

                #endregion
            }

        }
        #endregion

        #region CheckboxDelete


        private ICommand _checkBoxDel_Checked;
        public ICommand CheckBoxDel_Checked
        {
            get
            {
                return _checkBoxDel_Checked ?? (_checkBoxDel_Checked = new CommandHandler(() => CheckBoxDel_CheckedAction(), () => CanExecute));
            }
        }

        void CheckBoxDel_CheckedAction()
        {
            if (StaticProp.PropCheckBoxDelete == false)
            {
                StaticProp.PropCheckBoxDelete = true;
            }
            else
            {
                StaticProp.PropCheckBoxDelete = false;
            }
            
        }
        #endregion

        #region CheckBox  Video  Separate   Folder


        private ICommand _CheckBoxVideoSeparateFolder;
        public ICommand CheckBoxVideoSeparateFolder
        {
            get
            {
                return _CheckBoxVideoSeparateFolder ?? (_CheckBoxVideoSeparateFolder = new CommandHandler(() => CheckBoxVideoSeparateFolder_Action(), () => CanExecute));
            }
        }

        void CheckBoxVideoSeparateFolder_Action()
        {
            if (StaticProp.CheckBoxVideoSeparateFolder == false)
            {
                StaticProp.CheckBoxVideoSeparateFolder = true;
            }
            else
            {
                StaticProp.CheckBoxVideoSeparateFolder = false;
            }

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
                var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg",
                    "mp4", "avi", "mpg", "mpeg", "m2v", "mpg", "mp2", "mpeg", "mpe", "mpv", "mp4", "m4p", "m4v", "amv", "rmvb", "rm", "yuv", "wmv", "mov", "qt", "mng", "gifv", "gif", "ogv", "ogg", "vob", "flv", "mkv" };

                filesNames = GetShortFilesFrom(searchFolder, filters, false);
                TextBoxFotoCounter = TextBoxFotoCounterMethod<string>(filesNames);
                ProgressBarStatusMax = TextBoxFotoCounterMethod<int>(filesNames);
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

        #region Button OldStyleCommand

        private ICommand _oldStyleCommand;
        public ICommand OldStyleCommand
        {
            get
            {
                return _oldStyleCommand ?? (_oldStyleCommand = new CommandHandler(() => OldStyleAction(), () => CanExecute));
            }
        }

        void OldStyleAction()
        {
            if (Helper.IsWindowOpen<MainWindow>()) // Check if any Window of a certain Type(mainWindow) or if a Window with a certain name is open
            {
                System.Windows.MessageBox.Show("Window MainWindow already open", "Notify",0);
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                App.Current.Windows[0].Close();
            }
        }
        #endregion

        #region Button GridStyleCommand

        private ICommand _gridStyleCommand;
        public ICommand GridStyleCommand
        {
            get
            {
                return _gridStyleCommand ?? (_gridStyleCommand = new CommandHandler(() => GridStyleCommandAction(), () => CanExecute));
            }
        }

        void GridStyleCommandAction()
        {
            if (Helper.IsWindowOpen<Main_Grid_Style>()) // Check if any Window of a certain Type(mainWindow) or if a Window with a certain name is open
            {
                System.Windows.MessageBox.Show("Window Main_Grid_Style already open", "Notification", 0);
            }
            else
            {
                
                Main_Grid_Style mainWindow = new Main_Grid_Style();
                mainWindow.Show();
                App.Current.Windows[0].Close();
            }
        }
        #endregion


        void TextGenShowMethod(string income)
        {
            TextBoxGenShow = income;
        }
        
        T TextBoxFotoCounterMethod<T>(string[] income)
        {
            int count = 0;

            foreach (var item in income)
            {
                count++;
            }

            return (T)Convert.ChangeType(count, typeof(T));
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
