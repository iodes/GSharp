using GSharp.Compile;
using System.Windows;

namespace GSharpSample
{
    /// <summary>
    /// ConfigDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ConfigDialog : Window
    {
        #region 속성
        public GCompilerConfig Config { get; set; }
        #endregion

        public ConfigDialog()
        {
            InitializeComponent();
            Loaded += ConfigDialog_Loaded;
        }

        private void ConfigDialog_Loaded(object sender, RoutedEventArgs e)
        {
            textTitle.Text = Config.Title;
            textDescription.Text = Config.Description;
            textCompany.Text = Config.Company;
            textProduct.Text = Config.Product;
            textCopyright.Text = Config.Copyright;
            textTrademark.Text = Config.Trademark;
            textVersion.Text = Config.Version;
            textFileVersion.Text = Config.FileVersion;
            checkCompressed.IsChecked = Config.IsCompressed;
            checkEmbedded.IsChecked = Config.IsEmbedded;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Config.Title = textTitle.Text;
            Config.Description = textDescription.Text;
            Config.Company = textCompany.Text;
            Config.Product = textProduct.Text;
            Config.Copyright = textCopyright.Text;
            Config.Trademark = textTrademark.Text;
            Config.Version = textVersion.Text;
            Config.FileVersion = textFileVersion.Text;
            Config.IsCompressed = checkCompressed.IsChecked.Value;
            Config.IsEmbedded = checkEmbedded.IsChecked.Value;

            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
