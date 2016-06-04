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
using GSharp.Base.Cores;
using GSharp.Base.Scopes;
using GSharp.Extension;
using GSharp.Graphic.Objects;
using GSharp.Graphic.Objects.Strings;

namespace GSharp.Graphic.Scopes
{
    /// <summary>
    /// EventBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ControlEventBlock : ScopeBlock, IModuleBlock
    {
        #region Holes
        public override NextConnectHole NextConnectHole
        {
            get
            {
                return RealNextConnectHole;
            }
        }
        #endregion

        #region Objects
        public GCommand GCommand
        {
            get
            {
                return _GControlEvent.GCommand;
            }
        }

        public GControlEvent GControlEvent
        {
            get
            {
                _GControlEvent.Clear();
                
                List<GBase> content = NextConnectHole?.StatementBlock?.ToGObjectList();
                if (content == null)
                {
                    content = new List<GBase>();
                }

                foreach (GBase gbase in content)
                {
                    if (gbase is GStatement)
                    {
                        _GControlEvent.Append(gbase as GStatement);
                    }
                }

                return _GControlEvent;
            }
        }
        private GControlEvent _GControlEvent;

        public override GScope GScope
        {
            get
            {
                return GControlEvent;
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return new List<GBase> { GScope };
        }
        #endregion
        
        // Constructor
        public ControlEventBlock(GCommand command)
        {
            // Initialize Component
            InitializeComponent();

            _GControlEvent = new GControlEvent(command);

            // Initialize Hole List
            HoleList.Add(NextConnectHole);
            
            // Initialize Block
            InitializeBlock();
        }
    }
}
