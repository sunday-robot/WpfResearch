using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ListViewResearch2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataContextClass DataContextClass = new DataContextClass();

        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class DataContextClass
    {
        public ObservableCollection<ListViewItemData> ListViewItemsSource { get; private set; }

        public DataContextClass()
        {
            this.ListViewItemsSource = new ObservableCollection<ListViewItemData>();
            this.ListViewItemsSource.Add(new ListViewItemData { Column1 = "A1", Column2 = "B1" });
            this.ListViewItemsSource.Add(new ListViewItemData { Column1 = "A2", Column2 = "B2" });
            this.ListViewItemsSource.Add(new ListViewItemData { Column1 = "A3", Column2 = "B3" });
        }
    }

    public struct ListViewItemData
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
    }
}
