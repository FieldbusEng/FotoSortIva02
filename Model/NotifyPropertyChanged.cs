using System.ComponentModel;


namespace FotoSortIva02.Model
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));

                //if (propertyname == "TextBoxStatus" && TextBoxesModel.TextBoxStatus == "Process Finished!")
                //{ }
            }
        }



    }
}
