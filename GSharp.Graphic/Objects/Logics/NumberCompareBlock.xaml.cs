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
using GSharp.Base.Objects.Logics;
using GSharp.Base.Objects;

namespace GSharp.Graphic.Objects.Logics
{
    /// <summary>
    /// NumberCompareBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NumberCompareBlock : LogicBlock, ICompareBlock
    {
        public override List<BaseHole> HoleList
        {
            get
            {
                return _HoleList;
            }
        }
        private List<BaseHole> _HoleList;

        public GCompare<GNumber> GCompare
        {
            get
            {
                GNumber obj1 = NumberHole1?.NumberBlock?.GNumber;
                GNumber obj2 = NumberHole2?.NumberBlock?.GNumber;

                if (obj1 == null || obj2 == null)
                {
                    throw new ToObjectException("블럭이 완성되지 않았습니다.", this);
                }

                GCompare<GNumber>.ConditionType op = GetConditionType();

                return new GCompare<GNumber>(obj1, op, obj2);
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

        public NumberCompareBlock()
        {
            InitializeComponent();

            _HoleList = new List<BaseHole>()
            {
                NumberHole1,
                NumberHole2
            };

            InitializeBlock();
        }

        public GCompare<GNumber>.ConditionType GetConditionType()
        {
            switch (Operator.Text)
            {
                case "=":
                    return GCompare<GNumber>.ConditionType.EQUAL;

                case "≠":
                    return GCompare<GNumber>.ConditionType.NOT_EQUAL;

                case "≤":
                    return GCompare<GNumber>.ConditionType.LESS_THEN_OR_EQUAL;

                case "≥":
                    return GCompare<GNumber>.ConditionType.GREATER_THEN_OR_EQUAL;

                case "<":
                    return GCompare<GNumber>.ConditionType.LESS_THEN;

                case ">":
                    return GCompare<GNumber>.ConditionType.GREATER_THEN;

                default:
                    return GCompare<GNumber>.ConditionType.EQUAL;
            }
        }

        public string GetConditionString()
        {
            return GetConditionType().ToString();
        }
    }
}
