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
    /// LogicCompareBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogicCompareBlock : LogicBlock, ICompareBlock
    {
        public override List<BaseHole> HoleList
        {
            get
            {
                return _HoleList;
            }
        }
        private List<BaseHole> _HoleList;

        public GCompare<GLogic> GCompare
        {
            get
            {
                GLogic obj1 = LogicHole1?.LogicBlock?.GLogic;
                GLogic obj2 = LogicHole2?.LogicBlock?.GLogic;

                if (obj1 == null || obj2 == null)
                {
                    throw new ToObjectException("블럭이 완성되지 않았습니다.", this);
                }

                GCompare<GLogic>.ConditionType op = GetConditionType();

                return new GCompare<GLogic>(obj1, op, obj2);
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

        public LogicCompareBlock()
        {
            InitializeComponent();

            _HoleList = new List<BaseHole>()
            {
                LogicHole1,
                LogicHole2
            };

            InitializeBlock();
        }

        public GCompare<GLogic>.ConditionType GetConditionType()
        {
            switch (Operator.Text)
            {
                case "=":
                    return GCompare<GLogic>.ConditionType.EQUAL;

                case "≠":
                    return GCompare<GLogic>.ConditionType.NOT_EQUAL;

                default:
                    return GCompare<GLogic>.ConditionType.EQUAL;
            }
        }

        public string GetConditionString()
        {
            return GetConditionType().ToString();
        }
    }
}
