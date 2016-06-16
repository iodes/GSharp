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
using GSharp.Base.Objects;
using System.Xml;
using GSharp.Graphic.Controls;

namespace GSharp.Graphic.Statements
{
    /// <summary>
    /// IfBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class IfBlock : PrevStatementBlock, IContainChildBlock
    {
        public IfBlock()
        {
            InitializeComponent();
            InitializeBlock();
        }

        public GIf GIf
        {
            get
            {
                GObject logic = LogicHole?.LogicBlock?.GObject;
                if (logic == null)
                {
                    throw new ToObjectException("조건문 블럭이 완성되지 않았습니다.", this);
                }

                return new GIf(logic);
            }
        }

        public override List<GBase> ToGObjectList()
        {
            List<GBase> baseList = new List<GBase>();

            GIf gif = GIf;

            List<GBase> childList = ChildConnectHole?.StatementBlock?.ToGObjectList();
            if (childList != null)
            {
                foreach (GBase child in childList)
                {
                    gif.Append((GStatement)child);
                }
            }

            baseList.Add(gif);

            List<GBase> nextList = NextConnectHole?.StatementBlock?.ToGObjectList();
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
                return GIf;
            }
        }

        public override List<BaseHole> HoleList
        {
            get
            {
                return new List<BaseHole> { LogicHole, NextConnectHole, ChildConnectHole };
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

        protected override void SaveBlockAttribute(XmlWriter writer)
        {
            BlockUtils.SaveChildBlocks(writer, ChildConnectHole.AttachedBlock);
        }

        public static BaseBlock LoadBlockFromXml(XmlElement element, BlockEditor blockEditor)
        {
            var block = new IfBlock();

            XmlNode node = element.SelectSingleNode("Holes/Hole/Block");
            block.LogicHole.LogicBlock = LoadBlock(node as XmlElement, blockEditor) as ObjectBlock;

            XmlNode childList = element.SelectSingleNode("ChildBlocks");
            block.ChildConnectHole.StatementBlock = LoadChildBlocks(childList as XmlElement, blockEditor);

            return block;
        }
    }
}
