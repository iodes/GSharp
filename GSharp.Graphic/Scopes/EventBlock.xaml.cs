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
using GSharp.Base.Scopes;
using GSharp.Extension;

namespace GSharp.Graphic.Scopes
{
    /// <summary>
    /// MethodBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EventBlock : ScopeBlock, IModuleBlock
    {
        #region Holes
        // Hole List
        public override List<BaseHole> HoleList
        {
            get
            {
                return _HoleList;
            }
        }
        private List<BaseHole> _HoleList;

        // Next Connect Hole
        public override NextConnectHole NextConnectHole
        {
            get
            {
                return RealNextConnectHole;
            }
        }
        #endregion

        #region Objects
        // GCommand
        public GCommand GCommand
        {
            get
            {
                return _GCommand;
            }
        }
        private GCommand _GCommand;

        // GEvent
        public GEvent GEvent
        {
            get
            {
                return _GEvent;
            }
        }
        private GEvent _GEvent;

        // GScopre
        public override GScope GScope
        {
            get
            {
                return GEvent;
            }
        }

        // GObjectList
        public override List<GBase> GObjectList
        {
            get
            {
                return _GObjectList;
            }
        }
        private List<GBase> _GObjectList;
        #endregion

        #region Constructor
        // Constructor
        public EventBlock(GCommand command)
        {
            // Initialize Component
            InitializeComponent();

            // Initialize Hole List
            _HoleList = new List<BaseHole> { RealNextConnectHole };

            // Initialize Objects
            _GCommand = command;
            _GEvent = new GEvent(command);
            _GObjectList = new List<GBase> { GScope };

            StackContentText.Text = command.FriendlyName;

            // Initialize Events
            RealNextConnectHole.BlockAttached += RealNextConnectHole_BlockAttached;

            // Initialize Block
            InitializeBlock();
        }
        #endregion

        #region Events
        // RealNextConnectHole BlockAttached Event
        private void RealNextConnectHole_BlockAttached(BaseBlock block)
        {
            _GEvent.Content.Clear();

            List<GBase> content = RealNextConnectHole?.StatementBlock?.GObjectList;
            if (content == null) return;
            
            foreach (GBase gbase in content)
            {
                if (gbase is GStatement)
                {
                    _GEvent.Append(gbase as GStatement);
                }
            }
        }
        #endregion
    }
}
