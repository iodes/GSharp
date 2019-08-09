using CefSharp;
using CefSharp.WinForms;
using System.Windows.Forms;

namespace GSharp.Sample
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            // CEFSharp Initialize Logic
            Cef.Initialize(new CefSettings());
            var browser = new ChromiumWebBrowser("Datas/Sample.html");
            Controls.Add(browser);
        }
    }
}
