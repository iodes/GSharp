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
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Holes;
using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Base.Objects.Strings;

namespace GSharp.Graphic.Objects.Strings
{
    /// <summary>
    /// VariableBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StringVariableBlock : StringBlock, IVariableBlock
    {
        public string FriendlyName
        {
            get
            {
                return _FriendlyName;
            }
            set
            {
                _FriendlyName = value;
                VariableName.Text = value;
            }
        }
        private string _FriendlyName;

        public GStringVariable GStringVariable
        {
            get
            {
                return _GStringVariable;
            }
        }
        private GStringVariable _GStringVariable;

        public IVariable IVariable
        {
            get
            {
                return GStringVariable;
            }
        }

        public override GString GString
        {
            get
            {
                return GStringVariable;
            }
        }

        public override GObject GObject
        {
            get
            {
                return GStringVariable;
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return _GObjectList;
        }
        private List<GBase> _GObjectList;

        // 생성자
        public StringVariableBlock(string friendlyName, GStringVariable variable)
        {
            InitializeComponent();

            FriendlyName = friendlyName;
            _GStringVariable = variable;
            _GObjectList = new List<GBase> { GObject };

            InitializeBlock();
        }
    }
}
