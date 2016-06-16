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
using GSharp.Base.Objects.Customs;
using System.Xml;
using GSharp.Graphic.Controls;

namespace GSharp.Graphic.Objects.Customs
{
    /// <summary>
    /// NumberPropertyBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EnumBlock : CustomBlock, IModuleBlock
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
                return GEnum;
            }
        }

        public GEnum GEnum
        {
            get
            {
                return _GEnum;
            }
        }
        private GEnum _GEnum;

        public override List<GBase> ToGObjectList()
        {
             return _GObjectList;
        }

        public override GCustom GCustom
        {
            get
            {
                return GEnum;
            }
        }

        private List<GBase> _GObjectList;

        // 생성자
        public EnumBlock(GCommand command)
            : base(command.ObjectType)
        {
            InitializeComponent();

            _GCommand = command;
            _GEnum = new GEnum(command);
            _GObjectList = new List<GBase> { GObject };

            if (command.Optionals != null)
            {
                foreach(var opt in command.Optionals)
                {
                    EnumName.Items.Add(opt);
                }
            }

            EnumName.SelectedIndex = 0;

            InitializeBlock();
        }

        private void EnumName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _GEnum.SelectedIndex = (sender as ComboBox).SelectedIndex;
        }

        protected override void SaveBlockAttribute(XmlWriter writer)
        {
            BlockUtils.SaveGCommand(writer, GCommand);
        }

        public static BaseBlock LoadBlockFromXml(XmlElement element, BlockEditor blockEditor)
        {
            var command = BlockUtils.LoadGCommand(element);
            return new EnumBlock(command);
        }
    }
}
