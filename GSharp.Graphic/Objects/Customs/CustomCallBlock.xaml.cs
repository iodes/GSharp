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

namespace GSharp.Graphic.Objects.Customs
{
    /// <summary>
    /// CustomCallBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomCallBlock : CustomBlock, ICallBlock
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
                return GCustomCall;
            }
        }

        public GCustomCall GCustomCall
        {
            get
            {
                return _GCustomCall;
            }
        }
        private GCustomCall _GCustomCall;

        public override List<GBase> ToGObjectList()
        {
            return _GObjectList;
        }

        public override GCustom GCustom
        {
            get
            {
                return GCustomCall;
            }
        }

        public ICall ICall
        {
            get
            {
                return GCustomCall;
            }
        }

        private List<GBase> _GObjectList;

        // 생성자
        public CustomCallBlock(GCommand command)
            : base(command.ObjectType)
        {
            InitializeComponent();

            _GCommand = command;
            _GCustomCall = new GCustomCall(command);
            _GObjectList = new List<GBase> { GObject };

            PropertyName.Text = command.FriendlyName;

            InitializeBlock();
        }
    }
}
