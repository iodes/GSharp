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
using System.Xml;
using GSharp.Graphic.Controls;

namespace GSharp.Graphic.Objects.Logics
{
    /// <summary>
    /// CompareBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CompareBlock : LogicBlock
    {
        public override List<BaseHole> HoleList
        {
            get
            {
                return _HoleList;
            }
        }
        private List<BaseHole> _HoleList;

        public Dictionary<GCompare.ConditionType, string> ConditionList = new Dictionary<GCompare.ConditionType, string>()
        {
            { GCompare.ConditionType.EQUAL, "=" },
            { GCompare.ConditionType.NOT_EQUAL, "≠" },
            { GCompare.ConditionType.LESS_THEN_OR_EQUAL, "≤" },
            { GCompare.ConditionType.GREATER_THEN_OR_EQUAL, "≥" },
            { GCompare.ConditionType.LESS_THEN, "<" },
            { GCompare.ConditionType.GREATER_THEN, ">" },
        };

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
                
                return new GCompare(obj1, ConditionType, obj2);
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

        public CompareBlock()
        {
            InitializeComponent();

            _HoleList = new List<BaseHole>()
            {
                ObjectHole1,
                ObjectHole2
            };

            InitializeBlock();

            Operator.ItemsSource = ConditionList;
            Operator.Items.Refresh();
        }

        public CompareBlock(GCompare.ConditionType conditionType) : this()
        {
            var conditionString = GetConditionString(conditionType);
            Operator.SelectedItem = ConditionList.Where(e => e.Key == conditionType).First();
        }

        public GCompare.ConditionType ConditionType
        {
            get
            {
                return ((KeyValuePair<GCompare.ConditionType, string>)Operator.SelectedItem).Key;
            }
        }

        public string GetConditionString(GCompare.ConditionType conditionType)
        {
            return ConditionList[conditionType];
        }

        protected override void SaveBlockAttribute(XmlWriter writer)
        {
            writer.WriteAttributeString("Condition", ConditionType.ToString());
        }
        
        public static BaseBlock LoadBlockFromXml(XmlElement element, BlockEditor blockEditor)
        {
            CompareBlock block;
            GCompare.ConditionType conditionType;

            var conditionTypeString = element.GetAttribute("Condition");
            if (Enum.TryParse(conditionTypeString, out conditionType))
            {
                block = new CompareBlock(conditionType);
            }
            else {
                block = new CompareBlock();
            }

            XmlNodeList elementList = element.SelectNodes("Holes/Hole");
            block.ObjectHole1.ObjectBlock = LoadBlock(elementList[0].SelectSingleNode("Block") as XmlElement, blockEditor) as ObjectBlock;
            block.ObjectHole2.ObjectBlock = LoadBlock(elementList[1].SelectSingleNode("Block") as XmlElement, blockEditor) as ObjectBlock;

            return block;
        }

        protected override void DisableBlock()
        {
            base.DisableBlock();

            Operator.IsEnabled = false;
        }
    }
}
