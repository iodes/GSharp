using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using GSharp.Base.Statements;
using GSharp.Base.Objects;
using GSharp.Extension;
using GSharp.Base.Objects.Logics;

namespace GSharp.Graphic.Objects.Logics
{
    /// <summary>
    /// LogicCallBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogicCallBlock : LogicBlock, ICallBlock
    {
        public GCommand GCommand { get; set; }

        // Hole List
        public override List<BaseHole> HoleList
        {
            get
            {
                return _HoleList;
            }
        }
        private List<BaseHole> _HoleList;

        public GLogicCall GLogicCall
        {
            get
            {
                List<GObject> objectList = new List<GObject>();

                foreach (BaseHole hole in _HoleList)
                {
                    if (hole.ParentBlock != this) continue;

                    if (hole.AttachedBlock == null)
                    {
                        throw new ToObjectException(GCommand.FriendlyName + "블럭이 완성되지 않았습니다.", this);
                    }

                    if (hole is BaseObjectHole)
                    {
                        objectList.Add((hole as BaseObjectHole).BaseObjectBlock.ToGObjectList()[0] as GObject);
                    }
                }

                return new GLogicCall(GCommand, objectList.ToArray());
            }
        }

        public override GLogic GLogic
        {
            get
            {
                return GLogicCall;
            }
        }

        public override GObject GObject
        {
            get
            {
                return GLogicCall;
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return _GObjectList;
        }

        public ICall ICall
        {
            get
            {
                return GLogicCall;
            }
        }

        private List<GBase> _GObjectList;

        public LogicCallBlock(GCommand command)
        {
            InitializeComponent();

            GCommand = command;
            _HoleList = BlockUtils.SetContent(GCommand, WrapContent);

            _GObjectList = new List<GBase> { GLogic };

            InitializeBlock();
        }
    }
}
