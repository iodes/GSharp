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
    /// VariableBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StringVariableBlock : StringBlock
    {
        // 생성자
        public StringVariableBlock(GVariable variable)
        {
            InitializeComponent();

            _GVariable = variable;
            VariableName.Text = variable.Name;

            InitializeBlock();
        }
        
        public GVariable GVariable
        {
            get
            {
                return _GVariable;
            }
        }
        private GVariable _GVariable;

        public override GObject GObject
        {
            get
            {
                return GVariable;
            }
        }

        public override List<GBase> GObjectList
        {
            get
            {
                return new List<GBase> { GObject };
            }
        }
    }
}
