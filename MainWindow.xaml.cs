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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Example_INotifyDataErrorInfo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataItems dataItems;

        public MainWindow()
        {
            InitializeComponent();

            dataItems = TestData.CreateDataItems(4, 4);
            DataGridParent.ItemsSource = dataItems;
        }

        private void ToggleDataContext_Click(object sender, RoutedEventArgs e)
        {
            DataGridParent.ItemsSource = null;
            DataGridParent.ItemsSource = dataItems;
        }
    }
}
