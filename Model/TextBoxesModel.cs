using FotoSortIva02.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FotoSortIva02.Model
{
    public class TextBoxesModel : NotifyPropertyChanged
    {
        #region IsEnabledOpenFolderButton
        private bool isEnabledOpenFolderButton;
        public bool IsEnabledOpenFolderButton
        {
            get { return this.isEnabledOpenFolderButton; }
            set
            {
                isEnabledOpenFolderButton = value;
                RaisePropertyChange("IsEnabledOpenFolderButton");

            }
        }

        #endregion

        #region IsEnabledExitButton
        private bool isEnabledExitButton;
        public bool IsEnabledExitButton
        {
            get { return this.isEnabledExitButton; }
            set
            {
                isEnabledExitButton = value;
                RaisePropertyChange("IsEnabledExitButton");

            }
        }
        #endregion

        #region Visibility Buttons
        private Visibility visibilityStopButton;
        public Visibility VisibilityStopButton
        {
            get { return this.visibilityStopButton; }
            set
            {
                visibilityStopButton = value;
                RaisePropertyChange("VisibilityStopButton");

            }
        }

        private Visibility visibilityStartButton;
        public Visibility VisibilityStartButton
        {
            get { return this.visibilityStartButton; }
            set
            {
                visibilityStartButton = value;
                RaisePropertyChange("VisibilityStartButton");

            }
        }

        private Visibility visibilityResetButton;
        public Visibility VisibilityResetButton
        {
            get
            {
                return this.visibilityResetButton;
            }
            set
            {
                visibilityResetButton = value;
                RaisePropertyChange("VisibilityResetButton");
            }
        }

        #endregion

        #region ProgressBarStatus
        private Visibility progressBarStatusVisible;
        public Visibility ProgressBarStatusVisible
        {
            get
            {
                //return (ProgressBarStatusVisibleBool == true) ? Visibility.Visible : Visibility.Collapsed;
                return progressBarStatusVisible;
            }
            set
            {
                progressBarStatusVisible = value;
                RaisePropertyChange("ProgressBarStatusVisible");

            }
        }


        private int progressBarStatusValue;
        public int ProgressBarStatusValue
        {
            get { return this.progressBarStatusValue; }
            set
            {
                progressBarStatusValue = value;
                RaisePropertyChange("ProgressBarStatusValue");

            }
        }

        private int progressBarStatusMax;
        public int ProgressBarStatusMax
        {
            get { return this.progressBarStatusMax; }
            set
            {
                progressBarStatusMax = value;
                RaisePropertyChange("ProgressBarStatusMax");

            }
        }

        #endregion


        //#region CheckBoxDelete
        //private bool checkBoxDelete;
        //public bool CheckBoxDelete
        //{
        //    get
        //    {
        //        return checkBoxDelete;
        //    }
        //    set
        //    {
        //        checkBoxDelete = value;
                
        //    }
        //}

        //#endregion

        private string textBoxGenShow;
        public string TextBoxGenShow
        {
            get { return textBoxGenShow; }
            set
            {
                textBoxGenShow = value;
                RaisePropertyChange("TextBoxGenShow");
            }
        }

        private string textBoxFotoCounter;

        public string TextBoxFotoCounter
        {
            get { return textBoxFotoCounter; }
            set
            {
                textBoxFotoCounter = value;
                RaisePropertyChange("TextBoxFotoCounter");
            }
        }

        private string textBoxNewFolder;

        public string TextBoxNewFolder
        {
            get { return textBoxNewFolder; }
            set {
                textBoxNewFolder = value;
                RaisePropertyChange("TextBoxNewFolder");
            }
        }

        private string textBoxScanFolder;

        public string TextBoxScanFolder
        {
            get { return textBoxScanFolder; }
            set {
                textBoxScanFolder = value;
                RaisePropertyChange("TextBoxScanFolder");
            }
        }

        private string textBoxStatus;
        public string TextBoxStatus
        {
            get { return textBoxStatus; }
            set
            {
                textBoxStatus = value;
                RaisePropertyChange("TextBoxStatus");
                StatusPropertyChange();
            }
        }
        public void StatusPropertyChange()
        {
            if (textBoxStatus == "Process Finished!")
            {
                VisibilityResetButton = Visibility.Visible;
            }
        }

        private string textBoxInfo;
        public string TextBoxInfo
        {
            get { return textBoxInfo; }
            set
            {
                textBoxInfo = value;
                RaisePropertyChange("TextBoxInfo");
            }
        }

        //-----------------------------------------------------------------

        private string bottomTextBl_Text;
        public string BottomTextBl_Text
        {
            get { return bottomTextBl_Text; }
            set
            {
                bottomTextBl_Text = value;
                RaisePropertyChange("BottomTextBl_Text");
            }
        }

        #region   ChBoxCopyOnlyNew_IsChecked
        private bool chBoxCopyOnlyNew_IsChecked;
        public bool ChBoxCopyOnlyNew_IsChecked
        {
            get { return this.chBoxCopyOnlyNew_IsChecked; }
            set
            {
                chBoxCopyOnlyNew_IsChecked = value;
                RaisePropertyChange("ChBoxCopyOnlyNew_IsChecked");

            }
        }
        #endregion

        #region   ChBoxCopyVideo_IsChecked
        private bool chBoxCopyVideo_IsChecked;
        public bool ChBoxCopyVideo_IsChecked
        {
            get { return this.chBoxCopyVideo_IsChecked; }
            set
            {
                chBoxCopyVideo_IsChecked = value;
                RaisePropertyChange("ChBoxCopyVideo_IsChecked");

            }
        }
        #endregion

    }
}
