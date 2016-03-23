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
using GSharp.Base.Logics;

namespace GSharp.Graphic.Logics
{
    /// <summary>
    /// CompareBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CompareBlock : LogicBlock
    {
        private List<BaseHole> holeList;
        public override List<BaseHole> HoleList
        {
            get
            {
                return holeList;
            }
        }

        public GCompare GCompare
        {
            get
            {
                GObject obj1 = ObjectHole1?.ObjectBlock?.GObject;
                GObject obj2 = ObjectHole2?.ObjectBlock?.GObject;

                if (obj1 == null || obj2 == null)
                {
                    throw new ToObjectException("블럭이 완성되지 않았습니다.", this);
                }

                GCompare.ConditionType op = GetConditionType();

                return new GCompare(obj1, op, obj2);
            }
        }

        public override GLogic GLogic
        {
            get
            {
                return GCompare;
            }
        }

        public override List<GBase> GObjectList
        {
            get
            {
                return new List<GBase> { GLogic };
            }
        }

        public CompareBlock()
        {
            InitializeComponent();

            holeList = new List<BaseHole>()
            {
                ObjectHole1,
                ObjectHole2
            };
        }

        private GCompare.ConditionType GetConditionType()
        {
            switch (Operator.Text)
            {
                case "=":
                    return GCompare.ConditionType.EQUAL;

                case "!=":
                    return GCompare.ConditionType.NOT_EQUAL;

                case "≤":
                    return GCompare.ConditionType.LESS_THEN_OR_EQUAL;

                case "≥":
                    return GCompare.ConditionType.GREATER_THEN_OR_EQUAL;

                case "<":
                    return GCompare.ConditionType.LESS_THEN;

                case ">":
                    return GCompare.ConditionType.GREATER_THEN;

                default:
                    return GCompare.ConditionType.EQUAL;
            }
        }
    }
}
