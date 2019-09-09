﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FotoSortIva02.Model
{
    public class TextBoxesModel : NotifyPropertyChanged
    {
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
            }
        }
    }
}
