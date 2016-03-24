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

namespace GSharp.Graphic.Statements
{
    /// <summary>
    /// ExtensionBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CallBlock : PrevStatementBlock, IModuleBlock
    {
        private List<BaseHole> holeList;

        public GCommand Command { get; set; }

        public CallBlock(GCommand command)
        {
            InitializeComponent();
            Command = command;
            holeList = ModuleBlock.SetContent(command.FriendlyName, StackContent);
            holeList.Add(NextConnectHole);
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

        public GCall GCall
        {
            get
            {
                List<GObject> objectList = new List<GObject>();

                foreach (BaseHole hole in holeList)
                {
                    if (!(hole is NextConnectHole) && hole.Block == null)
                    {
                        throw new ToObjectException(Command.FriendlyName + " / " + hole.ToString() + "블럭이 완성되지 않았습니다.", this);
                    }

                    if (hole is BaseObjectHole)
                    {
                        objectList.Add((hole as BaseObjectHole).BaseObjectBlock.GObjectList[0] as GObject);
                    }
                }

                return new GCall(Command, objectList.ToArray());
            }
        }

        public override GStatement GStatement
        {
            get
            {
                return GCall;
            }
        }

        public override List<GBase> GObjectList
        {
            get
            {
                return new List<GBase> { GStatement };
            }
        }

        public override List<BaseHole> HoleList
        {
            get
            {
                return holeList;
            }
        }
    }
}
