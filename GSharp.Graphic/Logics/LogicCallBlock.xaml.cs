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
using GSharp.Graphic.Core;
using GSharp.Graphic.Holes;
using GSharp.Base.Cores;
using GSharp.Base.Logics;
using GSharp.Base.Statements;
using GSharp.Base.Objects;
using GSharp.Extension;

namespace GSharp.Graphic.Logics
{
    /// <summary>
    /// LogicCallBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogicCallBlock : LogicBlock, IModuleBlock
    {
        public GCommand Command { get; set; }

        private List<BaseHole> holeList = new List<BaseHole>();
        public override List<BaseHole> HoleList
        {
            get
            {
                return holeList;
            }
        }

        public GLogicCall GLogicCall
        {
            get
            {
                List<GObject> objectList = new List<GObject>();

                foreach (BaseHole hole in holeList)
                {
                    if (hole.Block == null)
                    {
                        throw new ToObjectException(Command.FriendlyName + "블럭이 완성되지 않았습니다.", this);
                    }

                    if (hole is BaseObjectHole)
                    {
                        objectList.Add((hole as BaseObjectHole).BaseObjectBlock.GObjectList[0] as GObject);
                    }
                }

                return new GLogicCall(Command, objectList.ToArray());
            }
        }

        public override GLogic GLogic
        {
            get
            {
                return GLogicCall;
            }
        }

        public override List<GBase> GObjectList
        {
            get
            {
                return new List<GBase> { GLogic };
            }
        }

        public LogicCallBlock()
        {
            InitializeComponent();
        }

        public LogicCallBlock(GCommand command) : this()
        {
            Command = command;
            holeList = ModuleBlock.SetContent(command.FriendlyName, StackContent);
        }
    }
}
