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
using GSharp.Base.Objects;
using GSharp.Extension;
using GSharp.Base.Objects.Numbers;
using GSharp.Base.Objects.Customs;
using System.Xml;
using GSharp.Graphic.Controls;

namespace GSharp.Graphic.Objects
{
    /// <summary>
    /// ObjectCallBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ObjectCallBlock : ObjectBlock, IModuleBlock
    {
        public GCommand GCommand
        {
            get
            {
                return _GCommand;
            }
        }
        private GCommand _GCommand;

        public override GObject GObject
        {
            get
            {
                return GCall;
            }
        }

        public GObjectCall GCall
        {
            get
            {
                var argumentList = new List<GObject>();

                foreach(var hole in HoleList)
                {
                    argumentList.Add((hole as BaseObjectHole)?.BaseObjectBlock?.ToGObjectList()[0] as GObject);
                }

                return new GObjectCall(GCommand, argumentList.ToArray());
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return _GObjectList;
        }

        private List<GBase> _GObjectList;

        public override List<BaseHole> HoleList
        {
            get
            {
                return _HoleList;
            }
        }
        private List<BaseHole> _HoleList;

        // 생성자
        public ObjectCallBlock(GCommand command)
        {
            InitializeComponent();

            _GCommand = command;

            _HoleList = BlockUtils.SetContent(command, WrapContent);
            _GObjectList = new List<GBase> { GObject };

            InitializeBlock();
        }

        protected override void SaveBlockAttribute(XmlWriter writer)
        {
            BlockUtils.SaveGCommand(writer, GCommand);
        }

        public static BaseBlock LoadBlockFromXml(XmlElement element, BlockEditor blockEditor)
        {
            GCommand command = BlockUtils.LoadGCommand(element);

            ObjectCallBlock block = new ObjectCallBlock(command);
            XmlNodeList elementList = element.SelectNodes("Holes/Hole");

            for (int i=0; i< block.HoleList.Count; i++)
            {
                BlockUtils.ConnectToHole(block.HoleList[i], LoadBlock(elementList[i].SelectSingleNode("Block") as XmlElement, blockEditor));
            }

            return block;
        }
    }
}
