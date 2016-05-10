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
using GSharp.Base.Objects;
using GSharp.Extension;

namespace GSharp.Graphic.Objects
{
    /// <summary>
    /// StringPropertyBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NumberCallBlock : NumberBlock, IModuleBlock
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
                return GObjectCall;
            }
        }

        public GObjectCall GObjectCall
        {
            get
            {
                return _GObjectCall;
            }
        }
        private GObjectCall _GObjectCall;

        public override List<GBase> GObjectList
        {
            get
            {
                return _GObjectList;
            }
        }
        private List<GBase> _GObjectList;

        // 생성자
        public NumberCallBlock(GCommand command)
        {
            InitializeComponent();

            _GCommand = command;
            _GObjectCall = new GObjectCall(command);
            _GObjectList = new List<GBase> { GObject };

            PropertyName.Text = command.FriendlyName;

            InitializeBlock();
        }
    }
}
