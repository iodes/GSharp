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

namespace GSharp.Graphic.Objects.Customs
{
    /// <summary>
    /// NumberPropertyBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomPropertyBlock : CustomBlock, IPropertyBlock
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
                return GCustomProperty;
            }
        }

        public GCustomProperty GCustomProperty
        {
            get
            {
                return _GCustomProperty;
            }
        }
        private GCustomProperty _GCustomProperty;

        public override List<GBase> ToGObjectList()
        {
             return _GObjectList;
        }

        public override GCustom GCustom
        {
            get
            {
                return GCustomProperty;
            }
        }

        public IProperty IProperty
        {
            get
            {
                return GCustomProperty;
            }
        }

        private List<GBase> _GObjectList;

        // 생성자
        public CustomPropertyBlock(GCommand command)
            : base(command.ObjectType)
        {
            InitializeComponent();

            _GCommand = command;
            _GCustomProperty = new GCustomProperty(command);
            _GObjectList = new List<GBase> { GObject };

            PropertyName.Text = command.FriendlyName;

            InitializeBlock();
        }
    }
}
