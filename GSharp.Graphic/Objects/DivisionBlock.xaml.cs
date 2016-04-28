using System.Collections.Generic;
using GSharp.Graphic.Core;
using GSharp.Graphic.Holes;
using GSharp.Base.Cores;
using GSharp.Base.Objects;

namespace GSharp.Graphic.Objects
{
    /// <summary>
    /// NumberBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DivisionBlock : NumberBlock
    {
        public override List<BaseHole> HoleList
        {
            get
            {
                return new List<BaseHole> { NumberHole1, NumberHole2 };
            }
        }

        public GCompute GCompute
        {
            get
            {
                GObject obj1 = NumberHole1.NumberBlock.GObject;
                GObject obj2 = NumberHole2.NumberBlock.GObject;

                return new GCompute(obj1, GCompute.OperatorType.DIVISION, obj2);
            }
        }

        public override GObject GObject
        {
            get
            {
                return GCompute;
            }
        }

        public override List<GBase> GObjectList
        {
            get
            {
                return new List<GBase> { GObject };
            }
        }

        public DivisionBlock()
        {
            InitializeComponent();
            InitializeBlock();
        }
    }
}
