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
using GSharp.Base.Objects.Numbers;

namespace GSharp.Graphic.Objects
{
    /// <summary>
    /// VariableBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class VariableBlock : SettableObjectBlock
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

        public override GObject GObject
        {
            get
            {
                return GVariable;
            }
        }

        public override GSettableObject GSettableObject
        {
            get
            {
                return GVariable;
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return _GObjectList;
        }

        public GVariable GVariable
        {
            get
            {
                return _GVariable;
            }
        }
        private GVariable _GVariable;

        private List<GBase> _GObjectList;
        
        // 생성자
        public VariableBlock(string friendlyName, GVariable variable)
        {
            InitializeComponent();

            FriendlyName = friendlyName;
            _GVariable = variable;
            _GObjectList = new List<GBase> { GObject };

            InitializeBlock();
        }
    }
}
