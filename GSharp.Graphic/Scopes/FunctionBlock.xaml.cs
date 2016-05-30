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

namespace GSharp.Graphic.Scopes
{
    /// <summary>
    /// MethodBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FunctionBlock : ScopeBlock
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
        // GFunction
        public GFunction GFunction
        {
            get
            {
                return _GFunction;
            }
        }
        private GFunction _GFunction;

        // GScope
        public override GScope GScope
        {
            get
            {
                return GFunction;
            }
        }

        // GObjectList
        public override List<GBase> ToGObjectList()
        {
            return _GObjectList;
        }
        private List<GBase> _GObjectList;
        #endregion

        #region Constructor
        // Constructor
        public FunctionBlock(GFunction function)
        {
            // Initialize Component
            InitializeComponent();

            // Initialize Hole List
            _HoleList = new List<BaseHole> { NextConnectHole };

            // Initialize Objects
            _GFunction = function;
            _GObjectList = new List<GBase> { GScope };

            StackContentText.Text = function.FunctionName;

            // Initialize Events
            NextConnectHole.BlockAttached += RealNextConnectHole_BlockChanged;
            NextConnectHole.BlockDetached += RealNextConnectHole_BlockChanged;

            // Initialize Block
            InitializeBlock();
        }
        #endregion

        #region Events
        // RealNextConnectHole BlockAttached & BlockDetached Event
        private void RealNextConnectHole_BlockChanged(BaseHole hole, BaseBlock block)
        {
            _GFunction.Content.Clear();

            List<GBase> content = NextConnectHole.StatementBlock?.ToGObjectList();
            if (content == null) return;

            foreach (GBase gbase in content)
            {
                if (gbase is GStatement)
                {
                    _GFunction.Append(gbase as GStatement);
                }
            }
        }
        #endregion
    }
}
