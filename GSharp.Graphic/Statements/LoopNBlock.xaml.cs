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
using GSharp.Base.Objects;

namespace GSharp.Graphic.Statements
{
    /// <summary>
    /// IFBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoopNBlock : PrevStatementBlock, IContainChildBlock
    {
        public LoopNBlock()
        {
            InitializeComponent();
            InitializeBlock();
        }

        public GLoop GLoop
        {
            get
            {
                GNumber obj = NumberHole?.NumberBlock?.GNumber;
                if (obj == null)
                {
                    throw new ToObjectException("반복문 블럭이 완성되지 않았습니다.", this);
                }

                GLoop loop = new GLoop(obj);

                List<GBase> childList = ChildConnectHole?.StatementBlock?.GObjectList;
                if (childList != null)
                {
                    foreach (GBase child in childList)
                    {
                        loop.Append((GStatement)child);
                    }
                }

                return loop;
            }
        }

        public override List<GBase> GObjectList
        {
            get
            {
                List<GBase> baseList = new List<GBase>() { GLoop };

                List<GBase> nextList = NextConnectHole?.StatementBlock?.GObjectList;
                if (nextList != null)
                {
                    baseList.AddRange(nextList);
                }

                return baseList;
            }
        }

        public override GStatement GStatement
        {
            get
            {
                return GLoop;
            }
        }

        public override List<BaseHole> HoleList
        {
            get
            {
                return new List<BaseHole> { NumberHole, NextConnectHole, ChildConnectHole };
            }
        }

        public override NextConnectHole NextConnectHole
        {
            get
            {
                return RealNextConnectHole;
            }
        }

        public NextConnectHole ChildConnectHole
        {
            get
            {
                return RealChildConnectHole;
            }
        }
    }
}
