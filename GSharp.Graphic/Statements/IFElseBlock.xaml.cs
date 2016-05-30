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
using GSharp.Base.Statements;
using GSharp.Base.Cores;

namespace GSharp.Graphic.Statements
{
    /// <summary>
    /// IfElseBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class IfElseBlock : PrevStatementBlock, IContainChildBlock
    {
        public IfElseBlock()
        {
            InitializeComponent();
            InitializeBlock();
        }

        public GIfElse GIfElse
        {
            get
            {
                GLogic logic = LogicHole?.LogicBlock?.GLogic;
                if (logic == null)
                {
                    throw new ToObjectException("조건문 블럭이 완성되지 않았습니다.", this);
                }

                return new GIfElse(logic);
            }
        }

        public override List<GBase> ToGObjectList()
        {
            var baseList = new List<GBase>();

            var gIfElse = GIfElse;

            var ifChildList = IfChildConnectHole?.StatementBlock?.ToGObjectList();
            if (ifChildList != null)
            {
                foreach (var child in ifChildList)
                {
                    gIfElse.AppendToIf(child as GStatement);
                }
            }

            var elseChildList = ElseChildConnectHole?.StatementBlock?.ToGObjectList();
            if (elseChildList != null)
            {
                foreach (var child in elseChildList)
                {
                    gIfElse.AppendToElse(child as GStatement);
                }
            }

            baseList.Add(gIfElse);

            var nextList = NextConnectHole?.StatementBlock?.ToGObjectList();
            if (nextList != null)
            {
                baseList.AddRange(nextList);
            }

            return baseList;
        }

        public override GStatement GStatement
        {
            get
            {
                return GIfElse;
            }
        }

        public override List<BaseHole> HoleList
        {
            get
            {
                return new List<BaseHole> { LogicHole, NextConnectHole, IfChildConnectHole, ElseChildConnectHole };
            }
        }

        public override NextConnectHole NextConnectHole
        {
            get
            {
                return RealNextConnectHole;
            }
        }

        public NextConnectHole IfChildConnectHole
        {
            get
            {
                return RealIfChildConnectHole;
            }
        }

        public NextConnectHole ElseChildConnectHole
        {
            get
            {
                return RealElseChildConnectHole;
            }
        }

        public NextConnectHole ChildConnectHole
        {
            get
            {
                return IfChildConnectHole;
            }
        }
    }
}
