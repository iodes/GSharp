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
using GSharp.Base.Scopes;
using GSharp.Extension;
using GSharp.Graphic.Objects;
using GSharp.Graphic.Objects.Strings;
using GSharp.Extension.Exports;
using GSharp.Graphic.Controls;

namespace GSharp.Graphic.Scopes
{
    /// <summary>
    /// EventBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ControlEventBlock : ScopeBlock
    {
        #region Holes
        public override NextConnectHole NextConnectHole
        {
            get
            {
                return RealNextConnectHole;
            }
        }
        #endregion

        #region Objects
        public GControlEvent GControlEvent
        {
            get
            {
                var control = SelectedControl.Value;
                var export = SelectedEvent;

                if (control == null || export == null)
                {
                    throw new ToObjectException("컨트롤 이벤트 블럭이 완성되지 않았습니다.", this);
                }

                var controlEvent = new GControlEvent(control, export);
                List<GBase> content = NextConnectHole?.StatementBlock?.ToGObjectList();
                if (content == null)
                {
                    content = new List<GBase>();
                }

                foreach (GBase gbase in content)
                {
                    if (gbase is GStatement)
                    {
                        controlEvent.Append(gbase as GStatement);
                    }
                }

                return controlEvent;
            }
        }
        
        public override GScope GScope
        {
            get
            {
                return GControlEvent;
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return new List<GBase> { GScope };
        }
        #endregion

        #region ComboBox Binding
        protected override void OnBlockEditorChange()
        {
            ControlName.ItemsSource = BlockEditor.GControlList;
            SelectedControl = BlockEditor.GControlList.First();
            ControlName.SelectedIndex = 0;
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

                var eventList = SelectedControl.Value.Exports.Where(e => e.ObjectType != typeof(void));
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
        #endregion

        // Constructor
        public ControlEventBlock()
        {
            // Initialize Component
            InitializeComponent();

            // Initialize Hole List
            HoleList.Add(NextConnectHole);

            // Initialize Block
            InitializeBlock();
        }
    }
}
