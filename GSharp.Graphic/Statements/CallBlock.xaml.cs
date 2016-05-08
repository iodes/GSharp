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
    /// ExtensionBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CallBlock : PrevStatementBlock, IModuleBlock
    {
        #region Holes
        // NextConnectHole
        public override NextConnectHole NextConnectHole
        {
            get
            {
                return RealNextConnectHole;
            }
        }

        // HoleList
        public override List<BaseHole> HoleList
        {
            get
            {
                return _HoleList;
            }
        }
        private List<BaseHole> _HoleList;
        #endregion

        #region Objects
        public GCommand GCommand
        {
            get
            {
                return _GCommand;
            }
        }
        private GCommand _GCommand;

        public GFunction GFunction
        {
            get
            {
                return _GFunction;
            }
        }
        private GFunction _GFunction;

        public GCall GCall
        {
            get
            {
                return _GCall;
            }
        }
        private GCall _GCall;

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
                List<GBase> baseList = new List<GBase> { GStatement };
                List<GBase> nextList = NextConnectHole?.StatementBlock?.GObjectList;
                if (nextList != null)
                {
                    baseList.AddRange(nextList);
                }

                return baseList;
            }
        }
        #endregion

        public CallBlock(GCommand command)
        {
            // Initiailze Component
            InitializeComponent();

            // Initialize Hole List
            _HoleList = ModuleBlock.SetContent(command.FriendlyName, command.Arguments, StackContent);

            // Initialize Objects
            _GCommand = command;

            var objectList = new List<GObject>();

            foreach (BaseHole hole in _HoleList)
            {
                hole.BlockAttached += ObjectHole_BlockChanged;
                hole.BlockDetached += ObjectHole_BlockChanged;
                objectList.Add(null);
            }
            _GCall = new GCall(command, objectList.ToArray());

            _HoleList.Add(NextConnectHole);

            // Initialize Block
            InitializeBlock();
        }

        public CallBlock(GFunction function)
        {
            // Initiailze Component
            InitializeComponent();

            // Initialize Function
            _GFunction = function;
            _GCall = new GCall(function);

            // Initialize Block
            InitializeBlock();
        }

        public override StatementBlock GetLastBlock()
        {
            if (NextConnectHole.StatementBlock == null)
            {
                return this;
            }

            return NextConnectHole.StatementBlock.GetLastBlock();
        }

        private void ObjectHole_BlockChanged(BaseBlock block)
        {
            int idx = 0;
            foreach (BaseHole hole in _HoleList)
            {
                _GCall.targetArguments[idx++] = (hole as BaseObjectHole).BaseObjectBlock?.GObject;
            }
        }
    }
}
