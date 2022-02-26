using System.Diagnostics;
using System.Windows;

namespace NewUploaderGuiTechnicalResearch
{
    // タイトルバーは出したいが、×ボタンは非表示としたい場合、以下のページで紹介されているような少し煩雑な実装が必要となるらしい。
    // ("TitlaBar.CloseButton.Visible=false"のようなことをxamlに書けばよいかと期待していたが全然違っていた…)
    // https://lifetime-engineer.com/wpf-titlebar-button-delete/

    // データバインディングではなく、従来から行われてきたような、GUIコントロールのプロパティ操作を行うようにしているが、この方が記述量が少なくて済みそう。
    // データバインディングは、ビジネスロジック実装とUIデザインを完全に分離したいときには意味があるということか?
    public partial class ChooseDeviceWindow : Window
    {
        public ChooseDeviceWindow()
        {
            InitializeComponent();

            // ↓TODO 本来はコンストラクタ引数でモデルを受け取り、そこから選択肢と、デフォルト値を取得する
            DeviceComboBox.Items.Add("FV3000");
            DeviceComboBox.Items.Add("FV4000");
            DeviceComboBox.SelectedIndex = 1;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var r = DeviceComboBox.SelectedIndex;
            // ↓TODO 本来はコンストラクタ引数で受け取ったモデルに設定を行う
            Debug.WriteLine($"selected item index = {r}");
            DialogResult = true;
            Close();
        }
    }
}
