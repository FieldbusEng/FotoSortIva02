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
        public ViewModelBase()
        {
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
            if (StaticProp.ScanningFolderPath != "empty" && StaticProp.CreateFolderPath != "empty")
            {

            }
            else
            {
                MessageBox.Show("You have to choose SCAN folder and NEW folder", "Help", MessageBoxButtons.OK);


            }
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
                filesNames = GetFilesFrom(searchFolder, filters, false);
                TextBoxFotoCounter=TextBoxFotoCounterMethod(filesNames);
                TextGenShowMethod(string.Join("\r\n", filesNames));
            }

        }

        // method to return all needed files in folder

        string[] GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
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
                string[] filename = Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption);
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
