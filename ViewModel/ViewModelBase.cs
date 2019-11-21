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
using System.Xml.Serialization;

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
            IsEnabledOpenFolderButton = false;
            IsEnabledExitButton = true;
            VisibilityStopButton = Visibility.Hidden;

        }
        #endregion

        #region Button Start - StartButtCommand

        private ICommand _startButtCommand;
        public ICommand StartButtCommand
        {
            get
            {
                return _startButtCommand ?? (_startButtCommand = new CommandHandler(() => StartButtAction(), () => CanExecute));
            }
        }

        // here i announce thread so it will be accessible from Start and Stop button
        ThreadStart secondPotok;
        Thread potok;

        void StartButtAction()
        {
            // here i will call second Thread
            secondPotok = new ThreadStart(StartButtonMethod);
            potok = new Thread(secondPotok);
            potok.Start();
        }

        object block = new object();
        void StartButtonMethod()
        {
            // disable button exit during run of the process
            IsEnabledExitButton = false;
            VisibilityStopButton = Visibility.Visible;
            // create dictionary to put all pairs what to Move where to Move and serialize it.
            var MovePicDictionary = new Dictionary<string, string>();

            lock (block)
            {
                // if delete files after copy is true
                if (StaticProp.PropCheckBoxDelete)
                {
                    #region Check Box Delete = true
                    
                    if (StaticProp.ScanningFolderPath != "empty" && StaticProp.CreateFolderPath != "empty")
                    {
                        // Change TextBoxStatus
                        TextBoxStatus = "Process Started";
                        // Progress bar visible
                        ProgressBarStatusVisible = Visibility.Visible;

                        Pathes pathing = new Pathes(StaticProp.ScanningFolderPath);

                        String searchFolder = pathing.InitialFolderPath;
                        var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
                                              
                        MethodsFromStartButton methods = new MethodsFromStartButton();
                        filesNames = methods.GetLongFilesFrom(searchFolder, filters, false);

                        var filtersVideo = new String[] { "mp4", "avi", "mpg", "mpeg", "m2v", "mp2", "mpe", "mpv", "m4p", "m4v", "amv", "rmvb", "rm", "yuv", "wmv", "mov", "qt", "mng", "gifv", "ogv", "ogg", "vob", "flv", "mkv" };
                        
                        filesNamesVideo = methods.GetLongFilesFrom(searchFolder, filtersVideo, false);

                        // ProgressBarStatusMax will be only pictures
                        ProgressBarStatusMax = filesNames.Length;

                        // i work with video files separately
                        // make filter for video files
                        if (StaticProp.CheckBoxVideoSeparateFolder)
                        {
                            //  ProgressBarStatusMax will be pictures + Video
                            ProgressBarStatusMax = filesNames.Length + filesNamesVideo.Length;

                            CopyVideoFiles copyvideoInst = new CopyVideoFiles(filesNamesVideo);
                            // toDo implement ProgressBar for video copying as well

                        }

                        foreach (string item in filesNames)
                        {
                            ProgressBarStatusValue++;

                            try
                            {
                                
                                using (ExifReader reader = new ExifReader(item))
                                {
                                    // if exif data exist (!=null)
                                    if (reader != null)
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
                                            if (Directory.Exists(extendedYearPath))
                                            {
                                                //check if such a month exist in the new folder
                                                if (Directory.Exists(extendedMonthPath))
                                                {
                                                    // Copy the file
                                                    string fileToCopy = item;
                                                    string destinationDirectory = extendedMonthPath + "\\";

                                                    MovePicDictionary.Add(fileToCopy, destinationDirectory);

                                                    //CopyMethods instanceCopy = new CopyMethods();
                                                    //instanceCopy.Move_FileNameExistsMethod(fileToCopy, destinationDirectory);
                                                }
                                                else
                                                { // if month not exist
                                                    Directory.CreateDirectory(extendedMonthPath);
                                                    string fileToCopy = item;
                                                    string destinationDirectory = extendedMonthPath + "\\";

                                                    MovePicDictionary.Add(fileToCopy, destinationDirectory);
                                                    //CopyMethods instanceCopy = new CopyMethods();
                                                    //instanceCopy.Move_FileNameExistsMethod(fileToCopy, destinationDirectory);

                                                }
                                            }
                                            else
                                            {
                                                // if year not exist
                                                Directory.CreateDirectory(extendedYearPath);
                                                Directory.CreateDirectory(extendedMonthPath);
                                                string fileToCopy = item;
                                                string destinationDirectory = extendedMonthPath + "\\";
                                                MovePicDictionary.Add(fileToCopy, destinationDirectory);

                                                //CopyMethods instanceCopy = new CopyMethods();
                                                //instanceCopy.Move_FileNameExistsMethod(fileToCopy, destinationDirectory);
                                            }
                                            Thread.Sleep(2);

                                        }
                                    }
                                    else
                                    {
                                        // nothing

                                    }

                                }


                            }
                            catch (Exception e) // this comes in case No Exif data found
                            {
                                string messageToWriteFailed = "Exception happen" + e.ToString();
                                LoggingTxtIva ll2 = new LoggingTxtIva(messageToWriteFailed);

                                // here i need to create directory in case Exif Data not exist
                                messageToWriteFailed = "No Exif data in the file";
                                LoggingTxtIva ll5 = new LoggingTxtIva(messageToWriteFailed);

                                string extendedNoExif = StaticProp.CreateFolderPath + "\\No_Date";
                                Directory.CreateDirectory(extendedNoExif);
                                // Copy the file
                                string fileToCopy = item;
                                string destinationDirectory = extendedNoExif + "\\";

                                MovePicDictionary.Add(fileToCopy, destinationDirectory);

                                //CopyMethods instanceCopy = new CopyMethods();
                                //instanceCopy.Move_FileNameExistsMethod(fileToCopy, destinationDirectory);

                            }
                        }
                    
                        // Change TextBoxStatus
                        TextBoxStatus = "Move process Started";
                    }
                    else
                    {
                        //if (StaticProp.ScanningFolderPath != "empty" && StaticProp.CreateFolderPath != "empty")
                        //MessageBox.Show("You have to choose SCAN folder and NEW folder", "Help", MessageBoxButtons.OK);
                        TextBoxStatus = "Folders not determined";

                    }

                    // Serialisation Process
                    SerialisationProcess_Binary makeSerialisation = new SerialisationProcess_Binary();
                    // Make Serialisation:
                    makeSerialisation.DoSerialisationB(MovePicDictionary);
                    Thread.Sleep(1000);

                    // Move Process through another thread - In case it hase influence to the access of the picture source files 
                    //object toPassDict = makeDeSerialisation.DoDeserialisation();
                    Thread t_potok = new Thread(() => MethodDeSerialisation());
                    t_potok.Start();
                    t_potok.Join();

                    #endregion
                }
                else
                {
                    #region In case of CheckBox Delete files after copying is False
                    // todo change belo line back
                    if (StaticProp.ScanningFolderPath != "empty" && StaticProp.CreateFolderPath != "empty")
                    //if (StaticProp.ScanningFolderPath != "empty")
                    {
                        // Change TextBoxStatus
                        TextBoxStatus = "Process Started";
                        // Progress bar visible
                        ProgressBarStatusVisible = Visibility.Visible;

                        Pathes pathing = new Pathes(StaticProp.ScanningFolderPath);

                        String searchFolder = pathing.InitialFolderPath;
                        var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
                        MethodsFromStartButton methods = new MethodsFromStartButton();
                        filesNames = methods.GetLongFilesFrom(searchFolder, filters, false);

                        var filtersVideo = new String[] { "mp4", "avi", "mpg", "mpeg", "m2v", "mp2", "mpe", "mpv", "m4p", "m4v", "amv", "rmvb", "rm", "yuv", "wmv", "mov", "qt", "mng", "gifv", "ogv", "ogg", "vob", "flv", "mkv" };
                        filesNamesVideo = methods.GetLongFilesFrom(searchFolder, filtersVideo, false);
                        
                        // ProgressBarStatusMax will be only pictures
                        ProgressBarStatusMax = filesNames.Length;

                        // i work with video files separately
                        // make filter for video files
                        if (StaticProp.CheckBoxVideoSeparateFolder)
                        {
                            //  ProgressBarStatusMax will be pictures + Video
                            ProgressBarStatusMax = filesNames.Length + filesNamesVideo.Length;
                            CopyVideoFiles copyvideoInst = new CopyVideoFiles(filesNamesVideo);
                            // toDo implement ProgressBar for video copying as well
                        }
                        
                        foreach (string item in filesNames)
                        {
                            ProgressBarStatusValue++;

                            try
                            {
                                using (ExifReader reader = new ExifReader(item))
                                {
                                    // if exif data exist (!=null)
                                    if (reader != null)
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
                                            if (Directory.Exists(extendedYearPath))
                                            {
                                                //check if such a month exist in the new folder
                                                if (Directory.Exists(extendedMonthPath))
                                                {
                                                    // Copy the file
                                                    string fileToCopy = item;
                                                    string destinationDirectory = extendedMonthPath + "\\";

                                                    CopyMethods instanceCopy = new CopyMethods();
                                                    instanceCopy.Copy_FileNameExistsMethod(fileToCopy, destinationDirectory);
                                                    //File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));

                                                }
                                                else
                                                {
                                                    Directory.CreateDirectory(extendedMonthPath);
                                                    string fileToCopy = item;
                                                    string destinationDirectory = extendedMonthPath + "\\";
                                                    CopyMethods instanceCopy = new CopyMethods();
                                                    instanceCopy.Copy_FileNameExistsMethod(fileToCopy, destinationDirectory);

                                                    //File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));
                                                }
                                            }
                                            else
                                            {
                                                
                                                Directory.CreateDirectory(extendedYearPath);
                                                Directory.CreateDirectory(extendedMonthPath);
                                                string fileToCopy = item;
                                                string destinationDirectory = extendedMonthPath + "\\";
                                                CopyMethods instanceCopy = new CopyMethods();
                                                instanceCopy.Copy_FileNameExistsMethod(fileToCopy, destinationDirectory);
                                                //File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));
                                            }

                                            Thread.Sleep(2);
                                        }

                                    }
                                    else
                                    {
                                        // nothing

                                    }
                                }
                            }
                            catch (Exception e) // this comes if exif has no INFO
                            {
                                string messageToWriteFailed = "Exception happen" + e.ToString();
                                LoggingTxtIva ll2 = new LoggingTxtIva(messageToWriteFailed);


                                // here i need to create directory in case Exif Data not exist
                                messageToWriteFailed = "No Exif data in the file";
                                LoggingTxtIva ll5 = new LoggingTxtIva(messageToWriteFailed);

                                string extendedNoExif = StaticProp.CreateFolderPath + "\\No_Date";
                                Directory.CreateDirectory(extendedNoExif);
                                // Copy the file
                                string fileToCopy = item;
                                string destinationDirectory = extendedNoExif + "\\";

                                CopyMethods instanceCopy = new CopyMethods();
                                instanceCopy.Copy_FileNameExistsMethod(fileToCopy, destinationDirectory);
                                //File.Copy(fileToCopy, destinationDirectory + Path.GetFileName("\\" + fileToCopy));

                            }

                        }


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
             
            // enable exit button when process is finished
            IsEnabledExitButton = true;
            
        }

        public void MethodDeSerialisation()
        {
            SerialisationProcess_Binary makeDeSerialisation = new SerialisationProcess_Binary();

            Dictionary<string, string> deserializedDict = makeDeSerialisation.DoDeserialisationB();

            MethodMovePic(deserializedDict);
        }         

        public void MethodMovePic(Dictionary<string, string> _dict)
        {
                       
                Dictionary<string, string> incomeDict = _dict;
                foreach (KeyValuePair<string, string> itemm in incomeDict)
                {
                    CopyMethods instanceCopy = new CopyMethods();
                    instanceCopy.Move_FileNameExistsMethod(itemm.Key, itemm.Value);
                }
                TextBoxStatus = "Getting Info/Folders Finished!";
            
            
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

        #region Button OpenFolderLocation
        

        private ICommand _openFolderLocation;
        public ICommand OpenFolderLocation
        {
            get
            {
                return _openFolderLocation ?? (_openFolderLocation = new CommandHandler(() => OpenFolderLocationAction(), () => CanExecute));
            }
        }

        void OpenFolderLocationAction()
        {

            var pathNewFolder = TextBoxNewFolder;
            Process.Start(pathNewFolder);
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


                // Change TextBoxStatus
                TextBoxStatus = "Folder New is choosen";

                // Enable the button Open Folder Location
                IsEnabledOpenFolderButton = true;

            }

            // Getting the free space of the target drive
            int freeSpaceInt = 0;
            string driveChoosen = (StaticProp.CreateFolderPath).Substring(0, 3);
            DriveInfo drive = new DriveInfo(driveChoosen);
            if (drive.IsReady && drive.Name == driveChoosen)
            {
                long freeSpace = drive.TotalFreeSpace;
                freeSpaceInt = (int)Math.Ceiling((double)freeSpace / (double)1000000000);

            }
            BottomTextBl_Text = BottomTextBl_Text + "   --- Free space on the Drive " + driveChoosen+" " + freeSpaceInt.ToString() + " GB";


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

            string[] longFilesNames;
            string sizeOfFiles;
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
                     "avi", "mpg", "mpeg", "m2v", "mpg", "mp2", "mpeg", "mpe", "mpv", "mp4", "m4p", "m4v", "amv", "rmvb", "rm", "yuv", "wmv", "mov", "qt", "mng", "gifv", "gif", "ogv", "ogg", "vob", "flv", "mkv" };
                MethodsFromStartButton methods = new MethodsFromStartButton();
                filesNames = methods.GetShortFilesFrom(searchFolder, filters, false);
                longFilesNames = methods.GetLongFilesFrom(searchFolder, filters, false);
                sizeOfFiles = methods.GetSizeOftheFiles(longFilesNames);
                TextBoxFotoCounter = TextBoxFotoCounterMethod<string>(filesNames);
                //ProgressBarStatusMax = TextBoxFotoCounterMethod<int>(filesNames);
                TextGenShowMethod(string.Join("\r\n", filesNames));

                BottomTextBl_Text = "Total Number of Files : " + TextBoxFotoCounter + "     Total memory weight : " + sizeOfFiles +" Mb.";
                

            }
            

            TextBoxStatus = "Scanning Finished";

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

        #region Binding InfoButtCommand
        private ICommand _infoButtCommand;
        public ICommand InfoButtCommand
        {
            get
            {
                return _infoButtCommand ?? (_infoButtCommand = new CommandHandler(() => InfoButtCommandAction(), () => CanExecute));
            }
        }

        void InfoButtCommandAction()
        {
            if (Helper.IsWindowOpen<InfoWindow>()) // Check if any Window of a certain Type(mainWindow) or if a Window with a certain name is open
            {
                System.Windows.MessageBox.Show("Window Info already open", "Notification", 0);
            }
            else
            {

                InfoWindow infoWindow = new InfoWindow();
                infoWindow.Show();

            }
        }
        #endregion

        #region StopButtCommand


        private ICommand stopButtCommand;
        public ICommand StopButtCommand
        {
            get
            {
                return stopButtCommand ?? (stopButtCommand = new CommandHandler(() => StopButtCommandAction(), () => CanExecute));
            }
        }

        void StopButtCommandAction()
        {
            
            potok.Abort();
            // enable exit button when process is finished
            IsEnabledExitButton = true;

        }
        #endregion

        //#region InfoCloseButtCommand
        //private ICommand _infoCloseButtCommand;
        //public ICommand InfoCloseButtCommand
        //{
        //    get
        //    {
        //        return _infoCloseButtCommand ?? (_infoCloseButtCommand = new CommandHandler(() => InfoCloseButtCommandAction(), () => CanExecute));
        //    }
        //}

        //void InfoCloseButtCommandAction()
        //{
        //    System.Windows.Application.Current.Windows[1].Close();
        //}
        //#endregion
    }
}
