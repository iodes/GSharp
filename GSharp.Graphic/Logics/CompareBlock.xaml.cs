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
        public CompareBlock()
        {
            InitializeComponent();
        }

        public override List<BaseHole> GetHoleList()
        {
            List<BaseHole> baseHoleList = new List<BaseHole>();

            baseHoleList.Add(ObjectHole1);
            baseHoleList.Add(ObjectHole2);

            return baseHoleList;
        }

        public override List<GBase> ToObject()
        {
            List<GBase> baseList = new List<GBase>();

            GObject obj1 = (GObject)ObjectHole1?.ObjectBlock?.ToObject()[0];
            GObject obj2 = (GObject)ObjectHole2?.ObjectBlock?.ToObject()[0];

            if (obj1 == null || obj2 == null)
            {
                throw new ToObjectException("비교 블럭이 완성되지 않았습니다.", this);
            }

            GCompare.ConditionType op;

            switch (Operator.Text)
            {
                case "=":
                    op = GCompare.ConditionType.EQUAL;
                    break;

                case "!=":
                    op = GCompare.ConditionType.NOT_EQUAL;
                    break;

                case "≤":
                    op = GCompare.ConditionType.LESS_THEN_OR_EQUAL;
                    break;

                case "≥":
                    op = GCompare.ConditionType.GREATER_THEN_OR_EQUAL;
                    break;

                case "<":
                    op = GCompare.ConditionType.LESS_THEN;
                    break;

                case ">":
                    op = GCompare.ConditionType.GREATER_THEN;
                    break;

                default:
                    op = GCompare.ConditionType.EQUAL;
                    break;
            }

            baseList.Add(new GCompare(obj1, obj2, op));

            return baseList;
        }
    }
}
