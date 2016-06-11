using System.Collections.Generic;
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Holes;
using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Base.Objects.Numbers;
using System;

namespace GSharp.Graphic.Objects.Numbers
{
    /// <summary>
    /// ComputeBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ComputeBlock : NumberBlock
    {
        #region Holes
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
        public GCompute GCompute
        {
            get
            {
                GObject obj1 = NumberHole1.NumberBlock?.GObject;
                GObject obj2 = NumberHole2.NumberBlock?.GObject;

                return new GCompute(obj1, OperatorType, obj2);
            }
        }

        public override GNumber GNumber
        {
            get
            {
                return GCompute;
            }
        }

        public override GObject GObject
        {
            get
            {
                return GCompute;
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return new List<GBase> { GObject };
        }
        #endregion

        #region Operators
        public GCompute.OperatorType OperatorType
        {
            get
            {
                return _OperatorType;
            }
        }

        private GCompute.OperatorType _OperatorType;
        #endregion

        #region Constructor
        public ComputeBlock(GCompute.OperatorType op)
        {
            // Initialize Component
            InitializeComponent();

            // Initialize Hole List
            _HoleList = new List<BaseHole> { NumberHole1, NumberHole2 };

            // Initialize Operator
            _OperatorType = op;

            string str = "";
            switch(op)
            {
                case GCompute.OperatorType.PLUS:
                    str = "+";
                    break;

                case GCompute.OperatorType.MINUS:
                    str = "-";
                    break;

                case GCompute.OperatorType.MULTIPLY:
                    str = "×";
                    break;

                case GCompute.OperatorType.DIVISION:
                    str = "÷";
                    break;

                case GCompute.OperatorType.MODULO:
                    str = "%";
                    break;
            }

            Operator.Text = str;

            // Initialize Block
            InitializeBlock();
        }
        #endregion
    }
}