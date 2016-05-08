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
using GSharp.Base.Objects;

namespace GSharp.Graphic.Objects
{
    /// <summary>
    /// MinusBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MinusBlock : NumberBlock
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
        // GCompute
        public GCompute GCompute
        {
            get
            {
                return _GCompute;
            }
        }
        private GCompute _GCompute;

        // GObject
        public override GObject GObject
        {
            get
            {
                return GCompute;
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
        public MinusBlock()
        {
            // Initialize Component
            InitializeComponent();

            // Initialize Lists
            _HoleList = new List<BaseHole> { NumberHole1, NumberHole2 };
            _GObjectList = new List<GBase> { GObject };

            // Initialize Objects
            _GCompute = new GCompute(null, GCompute.OperatorType.MINUS, null);

            // Initialize Event
            NumberHole1.BlockAttached += NumberHole1_BlockChanged;
            NumberHole1.BlockDetached += NumberHole1_BlockChanged;
            NumberHole2.BlockAttached += NumberHole2_BlockChanged;
            NumberHole2.BlockDetached += NumberHole2_BlockChanged;

            // Initialize Block
            InitializeBlock();
        }
        #endregion

        #region Event
        // NumberHole1 BlockAttached Event
        private void NumberHole1_BlockChanged(BaseBlock block)
        {
            GCompute.FirstPart = NumberHole1.NumberBlock?.GObject;
        }

        // NumberHole2 BlockAttached Event
        private void NumberHole2_BlockChanged(BaseBlock block)
        {
            GCompute.SecondPart = NumberHole2.NumberBlock?.GObject;
        }
        #endregion
    }
}
