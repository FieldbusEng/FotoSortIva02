using FotoSortIva02.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
