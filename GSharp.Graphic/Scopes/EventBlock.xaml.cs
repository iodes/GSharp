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
        #region Hole List
        // Hole List
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
        // GCommand
        public GCommand GCommand
        {
            get
            {
                return _GCommand;
            }
        }
        private GCommand _GCommand;

        public GEvent GEvent
        {
            get
            {
                List<GBase> content = RealNextConnectHole?.StatementBlock?.GObjectList;
                if (content == null)
                {
                    content = new List<GBase>();
                }

                GEvent evt = new GEvent(GCommand);

                foreach (GBase gbase in content)
                {
                    if (gbase is GStatement)
                    {
                        evt.Append(gbase as GStatement);
                    }
                }

                return evt;
            }
        }

        public override GScope GScope
        {
            get
            {
                return GEvent;
            }
        }

        public override List<GBase> GObjectList
        {
            get
            {
                return new List<GBase> { GScope };
            }
        }
        #endregion

        // Constructor
        public EventBlock(GCommand command)
        {
            // Initialize Component
            InitializeComponent();

            _GCommand = command;
            StackContentText.Text = command.FriendlyName;

            // Initialize Hole List
            _HoleList = new List<BaseHole> { RealNextConnectHole };

            // Initialize Block
            InitializeBlock();
        }
    }
}
