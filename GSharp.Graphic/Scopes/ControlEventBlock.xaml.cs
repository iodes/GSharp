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
using System.Xml;

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
                var control = SelectedControl.Key;
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
        private object backTarget;

        public KeyValuePair<string, GControl> SelectedControl
        {
            get
            {
                return _SelectedControl;
            }
            set
            {
                _SelectedControl = value;
                EventName.ItemsSource = SelectedControl.Value.Exports.Where(e => e.ObjectType == typeof(void));
                EventName.SelectedIndex = 0;
            }
        }
        private KeyValuePair<string, GControl> _SelectedControl;

        public GExport SelectedEvent { get; set; }

        private void UpdateSource()
        {
            backTarget = ControlName.SelectedItem;
            ControlName.ItemsSource = BlockEditor.GControlList;
            ControlName.Items.Refresh();

            if (ControlName.Items.Contains(backTarget))
            {
                ControlName.SelectedItem = backTarget;
            }
            else
            {
                ControlName.SelectedIndex = 0;
            }
        }

        protected override void OnBlockEditorChange()
        {
            UpdateSource();
            BlockEditor.PropertyChanged += (s, e) =>
            {
                UpdateSource();
            };
        }

        private void ControlName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ControlName.SelectedItem != null)
            {
                SelectedControl = (KeyValuePair<string, GControl>)ControlName.SelectedItem;
            }
        }

        private void EventName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EventName.SelectedItem != null)
            {
                SelectedEvent = (GExport)EventName.SelectedItem;

                AllowVariableList.Clear();
                ParameterBox.Children.Clear();

                for (int i = 0; i < SelectedEvent.Optionals?.Length; i++)
                {
                    VariableBlock variableBlock = BlockUtils.CreateVariableBlock(SelectedEvent.Optionals[i].Name, SelectedEvent.Optionals[i].FriendlyName);
                    BaseBlock baseBlock = variableBlock as BaseBlock;

                    baseBlock.MouseLeftButtonDown += BaseBlock_MouseLeftButtonDown;

                    AllowVariableList.Add(variableBlock);
                    ParameterBox.Children.Add(baseBlock);
                }
            }
        }
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

        protected override void SaveBlockAttribute(XmlWriter writer)
        {
            if (SelectedControl.Key?.Length > 0 && SelectedEvent.FullName?.Length > 0)
            {
                writer.WriteAttributeString("SelectedControlName", SelectedControl.Key);
                writer.WriteAttributeString("SelectedEventName", SelectedEvent.FullName);
            }
        }

        public static BaseBlock LoadBlockFromXml(XmlElement element, BlockEditor blockEditor)
        {
            ControlEventBlock block = new ControlEventBlock();
            block.BlockEditor = blockEditor;

            var controlName = element.GetAttribute("SelectedControlName");
            var eventName = element.GetAttribute("SelectedEventName");

            if (controlName.Length > 0)
            {
                var control = blockEditor.GControlList[controlName];
                block.ControlName.SelectedItem = control;
                
                var evtList = control.Exports.Where(e => e.ObjectType == typeof(void));
                var evt = evtList.Where(e => e.FullName == eventName).First();
                if (eventName.Length > 0)
                {
                    block.EventName.SelectedItem = evt;
                }
            }

            return block;
        }

        private void BaseBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var block = sender as VariableBlock;
            var copiedBlock = Activator.CreateInstance(block.GetType(), new object[] { block.FriendlyName, block.GVariable }) as BaseBlock;

            BlockEditor.AddBlock(copiedBlock);

            var masterPosition = e.GetPosition(BlockEditor.Master);
            var blockPosition = e.GetPosition(block as BaseBlock);

            copiedBlock.Position = new Point
            {
                X = masterPosition.X - blockPosition.X,
                Y = masterPosition.Y - blockPosition.Y
            };

            BlockEditor.StartBlockMove(copiedBlock, blockPosition);

            e.Handled = true;
        }

        protected override void DisableBlock()
        {
            base.DisableBlock();
            ParameterBox.Visibility = Visibility.Collapsed;
        }
    }
}
