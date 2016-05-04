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
using GSharp.Base.Statements;

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
            InitializeBlock();
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

        public GSet GSet
        {
            get
            {
                String variableName = VariableSelect.Text;
                GVariable variable = new GVariable(variableName);

                GObject obj = ObjectHole?.ObjectBlock?.GObject;

                if (obj == null)
                {
                    throw new ToObjectException("변수 설정 블럭이 완성되지 않았습니다.", this);
                }

                return new GSet(ref variable, obj);
            }
        }

        public override GStatement GStatement
        {
            get
            {
                return GSet;
            }
        }

        public override List<BaseHole> HoleList
        {
            get
            {
                return new List<BaseHole> { ObjectHole, NextConnectHole };
            }
        }

        public override List<GBase> GObjectList
        {
            get
            {
                List<GBase> baseList = new List<GBase>();
                baseList.Add(GSet);

                List<GBase> nextList = NextConnectHole?.StatementBlock?.GObjectList;
                if (nextList != null)
                {
                    baseList.AddRange(nextList);
                }

                return baseList;
            }
        }
    }
}
