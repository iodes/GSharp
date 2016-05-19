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
    /// NotBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NotBlock : LogicBlock
    {
        public override List<BaseHole> HoleList
        {
            get
            {
                return _HoleList;
            }
        }
        private List<BaseHole> _HoleList;

        public GNot GNot
        {
            get
            {
                GLogic logic = LogicHole1?.LogicBlock?.GLogic;

                if (logic == null)
                {
                    throw new ToObjectException("블럭이 완성되지 않았습니다.", this);
                }

                return new GNot(logic);
            }
        }

        public override GLogic GLogic
        {
            get
            {
                return GNot;
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return new List<GBase> { GLogic };
        }

        public NotBlock()
        {
            InitializeComponent();

            _HoleList = new List<BaseHole>() { LogicHole1 };

            InitializeBlock();
        }
    }
}
