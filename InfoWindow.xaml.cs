using FotoSortIva02.Resources;
using FotoSortIva02.ViewModel;
using System.Windows;

namespace FotoSortIva02
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
            TextBoxInfo.Text = StaticProp.initTextBoxInfo;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
