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
using GSharp.Extension.Exports;

namespace GSharp.Graphic.Objects
{
    /// <summary>
    /// ControlPropertyBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ControlPropertyBlock : SettableObjectBlock
    {
        public override GObject GObject
        {
            get
            {
                return GControlProperty;
            }
        }

        public override GSettableObject GSettableObject
        {
            get
            {
                return GControlProperty;
            }
        }

        public GControlProperty GControlProperty
        {
            get
            {
                var export = SelectedEvent;

                if (export == null)
                {
                    throw new ToObjectException("Event가 선택되지 않았습니다.", this);
                }

                return new GControlProperty(export);
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return new List<GBase>() { GControlProperty };
        }


        protected override void OnBlockEditorChange()
        {
            ControlName.ItemsSource = BlockEditor.GControlList;
            if (BlockEditor.GControlList.Count > 0)
            {
                SelectedControl = BlockEditor.GControlList.First();
                ControlName.SelectedIndex = 0;
            }
        }

        public KeyValuePair<string, GControl> SelectedControl
        {
            get
            {
                return _SelectedControl;
            }
            set
            {
                _SelectedControl = value;

                var eventList = SelectedControl.Value.Exports.Where(e => e.ObjectType == typeof(void));
                EventName.ItemsSource = eventList;
                if (eventList.Count() > 0)
                {
                    SelectedEvent = eventList.First();
                    EventName.SelectedIndex = 0;
                }
            }
        }
        private KeyValuePair<string, GControl> _SelectedControl;

        public GExport SelectedEvent { get; set; }

        // 생성자
        public ControlPropertyBlock()
        {
            InitializeComponent();

            InitializeBlock();
        }
    }
}
