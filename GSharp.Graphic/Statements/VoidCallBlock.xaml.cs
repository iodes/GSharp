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
using GSharp.Base.Cores;
using GSharp.Graphic.Core;
using GSharp.Graphic.Holes;
using GSharp.Base.Statements;
using GSharp.Graphic.Objects;
using GSharp.Base.Objects;
using GSharp.Extension;
using GSharp.Base.Scopes;

namespace GSharp.Graphic.Statements
{
    /// <summary>
    /// VoidCallBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class VoidCallBlock : PrevStatementBlock, ICallBlock
    {
        public override List<BaseHole> HoleList
        {
            get
            {
                return _HoleList;
            }
        }
        private List<BaseHole> _HoleList;

        public GCommand GCommand
        {
            get
            {
                return _GCommand;
            }
        }
        private GCommand _GCommand;

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

        public ICall ICall
        {
            get
            {
                return GVoidCall;
            }
        }

        public GVoidCall GVoidCall
        {
            get
            {
                List<GObject> objectList = new List<GObject>();

                foreach (BaseHole hole in HoleList)
                {
                    if (hole.ParentBlock != this) continue;

                    if (!(hole is NextConnectHole) && hole.AttachedBlock == null)
                    {
                        throw new ToObjectException(GCommand.FriendlyName + " / " + hole.ToString() + "블럭이 완성되지 않았습니다.", this);
                    }

                    if (hole is BaseObjectHole)
                    {
                        objectList.Add((hole as BaseObjectHole).BaseObjectBlock.GObjectList[0] as GObject);
                    }
                }

                return new GVoidCall(GCommand, objectList.ToArray());
            }
        }

        public override GStatement GStatement
        {
            get
            {
                return GVoidCall;
            }
        }

        public override List<GBase> GObjectList
        {
            get
            {
                List<GBase> baseList = new List<GBase> { GStatement };
                List<GBase> nextList = NextConnectHole?.StatementBlock?.GObjectList;
                if (nextList != null)
                {
                    baseList.AddRange(nextList);
                }

                return baseList;
            }
        }

        public VoidCallBlock(GCommand command)
        {
            InitializeComponent();

            _GCommand = command;

            _HoleList = ModuleBlock.SetContent(command.FriendlyName, command.Arguments, WrapContent);
            _HoleList.Add(NextConnectHole);

            InitializeBlock();
        }
    }
}
