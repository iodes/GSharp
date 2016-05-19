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
using GSharp.Base.Objects.Logics;
using GSharp.Base.Objects;

namespace GSharp.Graphic.Objects.Logics
{
    /// <summary>
    /// StringCompareBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StringCompareBlock : LogicBlock, ICompareBlock
    {
        public override List<BaseHole> HoleList
        {
            get
            {
                return _HoleList;
            }
        }
        private List<BaseHole> _HoleList;

        public GCompare<GString> GCompare
        {
            get
            {
                GString obj1 = StringHole1?.StringBlock?.GString;
                GString obj2 = StringHole2?.StringBlock?.GString;

                if (obj1 == null || obj2 == null)
                {
                    throw new ToObjectException("블럭이 완성되지 않았습니다.", this);
                }

                GCompare<GString>.ConditionType op = GetConditionType();

                return new GCompare<GString>(obj1, op, obj2);
            }
        }

        public override GLogic GLogic
        {
            get
            {
                return GCompare;
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return new List<GBase> { GLogic };
        }

        public StringCompareBlock()
        {
            InitializeComponent();

            _HoleList = new List<BaseHole>()
            {
                StringHole1,
                StringHole2
            };

            InitializeBlock();
        }

        public GCompare<GString>.ConditionType GetConditionType()
        {
            switch (Operator.Text)
            {
                case "=":
                    return GCompare<GString>.ConditionType.EQUAL;

                case "≠":
                    return GCompare<GString>.ConditionType.NOT_EQUAL;

                case "≤":
                    return GCompare<GString>.ConditionType.LESS_THEN_OR_EQUAL;

                case "≥":
                    return GCompare<GString>.ConditionType.GREATER_THEN_OR_EQUAL;

                case "<":
                    return GCompare<GString>.ConditionType.LESS_THEN;

                case ">":
                    return GCompare<GString>.ConditionType.GREATER_THEN;

                default:
                    return GCompare<GString>.ConditionType.EQUAL;
            }
        }

        public string GetConditionString()
        {
            return GetConditionType().ToString();
        }
    }
}
