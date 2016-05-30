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
using GSharp.Base.Objects.Strings;

namespace GSharp.Graphic.Objects.Strings
{
    /// <summary>
    /// StringPropertyBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StringPropertyBlock : StringBlock, IModuleBlock
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
                return GStringProperty;
            }
        }

        public override GString GString
        {
            get
            {
                return GStringProperty;
            }
        }

        public GStringProperty GStringProperty
        {
            get
            {
                return _GStringProperty;
            }
        }
        private GStringProperty _GStringProperty;

        public override List<GBase> ToGObjectList()
        {
            return _GObjectList;
        }
        private List<GBase> _GObjectList;

        // 생성자
        public StringPropertyBlock(GCommand command)
        {
            InitializeComponent();

            _GCommand = command;
            _GStringProperty = new GStringProperty(command);
            _GObjectList = new List<GBase> { GObject };

            PropertyName.Text = command.FriendlyName;

            InitializeBlock();
        }
    }
}
