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
using GSharp.Cores;
using GSharp.Objects;
using GSharp.Statements;

namespace GSharp.Graphic.Statements
{
    /// <summary>
    /// SetBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SetBlock : PrevStatementBlock
    {
        public SetBlock()
        {
            InitializeComponent();
        }

        public override NextConnectHole NextConnectHole
        {
            get
            {
                return RealNextConnectHole;
            }
        }

        public override StatementBlock GetLastBlock()
        {
            if (NextConnectHole.StatementBlock == null)
            {
                return this;
            }

            return NextConnectHole.StatementBlock.GetLastBlock();
        }

        public override List<BaseHole> GetHoleList()
        {
            List<BaseHole> baseHoleList = new List<BaseHole>();

            baseHoleList.Add(ObjectHole);
            baseHoleList.Add(PrevConnectHole);
            baseHoleList.Add(NextConnectHole);

            return baseHoleList;
        }

        public override List<GBase> ToObject()
        {
            List<GBase> baseList = new List<GBase>();

            String variableName = VariableSelect.Text;
            GVariable variable = new GVariable(variableName);

            GObject obj = (GObject)ObjectHole?.ObjectBlock?.ToObject()[0];
             
            if (obj == null)
            {
                throw new ToObjectException("변수 설정 블럭이 완성되지 않았습니다.", this);
            }

            baseList.Add(new GSet(ref variable, obj));

            List<GBase> nextList = NextConnectHole?.StatementBlock?.ToObject();

            if (nextList != null)
            {
                baseList.AddRange(nextList);
            }

            return baseList;
        }
    }
}
