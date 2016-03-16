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
using GSharp.Base.Statements;
using GSharp.Base.Cores;
using GSharp.Base.Logics;

namespace GSharp.Graphic.Statements
{
    /// <summary>
    /// IFBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class IFBlock : PrevStatementBlock
    {
        public List<StatementBlock> Children = new List<StatementBlock>();

        public IFBlock()
        {
            InitializeComponent();

            GStatement = new GIF(null);
        }

        public override NextConnectHole NextConnectHole
        {
            get
            {
                return RealNextConnectHole;
            }
        }

        public override List<BaseHole> GetHoleList()
        {
            List<BaseHole> baseHoleList = new List<BaseHole>();

            baseHoleList.Add(LogicHole);
            baseHoleList.Add(NextConnectHole);
            baseHoleList.Add(ChildConnectHole);

            return baseHoleList;
        }

        public override List<GBase> ToObject()
        {
            List<GBase> baseList = new List<GBase>();

            GLogicCall logic = (GLogicCall)LogicHole?.LogicBlock?.ToObject()[0];
            if (logic == null)
            {
                throw new ToObjectException("조건문 블럭이 완성되지 않았습니다.", this);
            }

            GIF gIF = new GIF(logic);

            List<GBase> childList = ChildConnectHole?.StatementBlock?.ToObject();
            if (childList != null)
            { 
                foreach(GBase child in childList)
                {
                    gIF.Append((GStatement)child);
                }
            }
            baseList.Add(gIF);
            
            List<GBase> nextList = NextConnectHole?.StatementBlock?.ToObject();
            if (nextList != null)
            {
                baseList.AddRange(nextList);
            }

            return baseList;
        }
    }
}
