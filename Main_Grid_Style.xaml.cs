using FotoSortIva02.ViewModel;
using System.Windows;
using System.Windows.Input;


namespace FotoSortIva02
{
    /// <summary>
    /// Interaction logic for Main_Grid_Style.xaml
    /// </summary>
    public partial class Main_Grid_Style : Window
    {
        public Main_Grid_Style()
        {
            InitializeComponent();

            var instance = ViewModelBase.Instance;
            //instance.name_singleton = "Main_Grid_Style singleton";

            DataContext = instance;
        }

        // this method allow to move window by dragging the text block <TextBlock Text="Foto Sorting App" 
        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
