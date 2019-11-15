using FotoSortIva02.ViewModel;
using System.Windows;

namespace FotoSortIva02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var instance = ViewModelBase.Instance;
            instance.name_singleton = "MainWindow singleton";

            DataContext =instance;

        }
    }
}
