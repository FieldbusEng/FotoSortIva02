using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoSortIva02.Model
{
    public class TextBoxesModel : NotifyPropertyChanged
    {
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
