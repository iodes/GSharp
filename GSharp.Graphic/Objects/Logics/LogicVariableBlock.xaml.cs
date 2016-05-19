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
using GSharp.Base.Objects.Logics;

namespace GSharp.Graphic.Objects.Logics
{
    /// <summary>
    /// VariableBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogicVariableBlock : LogicBlock, IVariableBlock
    {   
        public override GLogic GLogic
        {
            get
            {
                return GLogicVariable;
            }
        }

        public GLogicVariable GLogicVariable
        {
            get
            {
                return _GLogicVariable;
            }
        }
        private GLogicVariable _GLogicVariable;

        public override GObject GObject
        {
            get
            {
                return GLogicVariable;
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return _GObjectList;
        }

        public IVariable IVariable
        {
            get
            {
                return GLogicVariable;
            }
        }

        private List<GBase> _GObjectList;
        
        // 생성자
        public LogicVariableBlock(GLogicVariable variable)
        {
            InitializeComponent();

            _GLogicVariable = variable;
            VariableName.Text = variable.Name;
            _GObjectList = new List<GBase> { GObject };

            InitializeBlock();
        }
    }
}
