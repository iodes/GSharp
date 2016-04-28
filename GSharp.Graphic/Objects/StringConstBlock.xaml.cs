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
using GSharp.Graphic.Core;
using GSharp.Graphic.Holes;
using GSharp.Base.Cores;
using GSharp.Base.Objects;

namespace GSharp.Graphic.Objects
{
    /// <summary>
    /// NumberBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StringConstBlock : StringBlock
    {
        public string String
        {
            get
            {
                return StringText.Text;
            }
            set
            {
                StringText.Text = value;
            }
        }

        public GString GString
        {
            get
            {
                return new GString(String);
            }
        }

        public override GObject GObject
        {
            get
            {
                return GString;
            }
        }

        public override List<GBase> GObjectList
        {
            get
            {
                return new List<GBase> { GObject };
            }
        }

        public StringConstBlock()
        {
            InitializeComponent();
            InitializeBlock();
        }
    }
}
