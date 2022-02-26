using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace NewUploaderGuiTechnicalResearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum UploadResult
        {
            Successed,
            Failed,
            Canceled
        }

        class UploadFile : INotifyPropertyChanged
        {
            static readonly ImageSource SuccessedIconImageSource = new BitmapImage(new Uri("Resources/successed.png", UriKind.Relative));
            static readonly ImageSource FailedIconImageSource = new BitmapImage(new Uri("Resources/failed.png", UriKind.Relative));
            static readonly ImageSource CanceledIconImageSource = new BitmapImage(new Uri("Resources/canceled.png", UriKind.Relative));
            public string Name { get; }
            public string SizeMb { get; }

            private UploadResult? m_result = null;
            private string m_resultDescription = null;

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public UploadResult? Result => m_result;
            public void SetResult(UploadResult value, string description = "")
            {
                m_result = value;
                m_resultDescription = description;
                NotifyPropertyChanged();
                NotifyPropertyChanged("ResultIcon");
                NotifyPropertyChanged("ResultDescription");
                NotifyPropertyChanged("DeleteButtonVisibility");
            }

            public ImageSource ResultIcon
            {
                get
                {
                    if (Result == null)
                        return null;
                    switch (Result.Value)
                    {
                        case UploadResult.Successed:
                            return SuccessedIconImageSource;
                        case UploadResult.Failed:
                            return FailedIconImageSource;
                        default:
                            return CanceledIconImageSource;
                    }
                }
            }

            public string ResultDescription => m_resultDescription;

            public Visibility DeleteButtonVisibility
            {
                get
                {
                    return Result == null ? Visibility.Visible : Visibility.Hidden;
                }
            }

            public UploadFile(string name, long size)
            {
                Name = name;
                SizeMb = string.Format("{0}.{1} MB", size / 1_000_000, size / 100_000 % 10);
            }
        }

        readonly ObservableCollection<UploadFile> m_uploadFileList = new ObservableCollection<UploadFile>();

        public MainWindow()
        {
            InitializeComponent();

            var ul1 = new UploadFile("aaa.oir", 123_456_789);
            ul1.SetResult(UploadResult.Successed);
            var ul2 = new UploadFile("bbb.oir", 4_567_890_123_456);
            ul2.SetResult(UploadResult.Failed, "storage limit error");
            var ul3 = new UploadFile("ccc.oir", 123);
            ul3.SetResult(UploadResult.Canceled);
            var ul4 = new UploadFile("ddd.oir", 123_456);
            var ul5 = new UploadFile("eee.oir", 123_456_678);
            m_uploadFileList.Add(ul1);
            m_uploadFileList.Add(ul2);
            m_uploadFileList.Add(ul3);
            m_uploadFileList.Add(ul4);
            m_uploadFileList.Add(ul5);
            UploadFileListView.ItemsSource = m_uploadFileList;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Debug.WriteLine(sender);
            Debug.WriteLine(e);
            //System.Diagnostics.Process.Start(e.Uri.ToString());
            _ = Process.Start("https://www.yahoo.co.jp");
        }

        private void ChangeDeviceButton_Click(object sender, RoutedEventArgs e)
        {
            var w = new ChooseDeviceWindow();
            var r = w.ShowDialog();
            Debug.WriteLine($"ChooseDeviceWindow result = {r}");
        }

        private void DropAreaGrid_Drop(object sender, DragEventArgs e)
        {
            var pathList = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var path in pathList)
            {
                Debug.WriteLine(path);
            }

            DropAreaGrid.Visibility = Visibility.Hidden;
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {

            DropAreaGrid.Visibility = Visibility.Visible;
        }

        private void UploadFileDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;
            var dc = (UploadFile)b.DataContext;
            m_uploadFileList.Remove(dc);
        }

        private void DeleteProcessedUploadFilesButton_Click(object sender, RoutedEventArgs e)
        {
            var removeItems = m_uploadFileList.Where(e => e.Result != null).ToList();
            foreach (var item in removeItems)
            {
                m_uploadFileList.Remove(item);
            }
        }

        private void UploadFileListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            var ul = (UploadFile)e.AddedItems[0];
            if (ul.Result == null)
            {
                ul.SetResult(UploadResult.Failed, "Authorization error.");
            }
        }
    }
}
