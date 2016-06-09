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

namespace GSharp.Graphic.Scopes
{
    /// <summary>
    /// EventBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EventBlock : ScopeBlock, IModuleBlock
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
        // GCommand
        public GCommand GCommand
        {
            get
            {
                return GEvent.GCommand;
            }
        }

        public GEvent GEvent
        {
            get
            {
                _GEvent.Clear();

                List<GBase> content = NextConnectHole?.StatementBlock?.ToGObjectList();
                if (content == null)
                {
                    content = new List<GBase>();
                }
                
                foreach (GBase gbase in content)
                {
                    if (gbase is GStatement)
                    {
                        _GEvent.Append(gbase as GStatement);
                    }
                }

                return _GEvent;
                
            }
        }
        private GEvent _GEvent;

        public override GScope GScope
        {
            get
            {
                return GEvent;
            }
        }

        public override List<GBase> ToGObjectList()
        {
            return new List<GBase> { GScope };
        }
        #endregion
        
        // Constructor
        public EventBlock(GCommand command)
        {
            // Initialize Component
            InitializeComponent();

            StackContentText.Text = command.FriendlyName;

            _GEvent = new GEvent(command);

            // Initialize Hole List
            HoleList.Add(NextConnectHole);
            
            for (int i=0; i<_GEvent.Arguments?.Count; i++)
            {
                VariableBlock variableBlock = BlockUtils.CreateVariableBlock(_GEvent.Arguments[i].Name, command.Optionals[i].FriendlyName);
                BaseBlock baseBlock = variableBlock as BaseBlock;

                baseBlock.MouseLeftButtonDown += BaseBlock_MouseLeftButtonDown;

                AllowVariableList.Add(variableBlock);
                ParameterBox.Children.Add(baseBlock);
            }

            // Initialize Block
            InitializeBlock();
        }

        private void BaseBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var block = sender as VariableBlock;
            var copiedBlock = Activator.CreateInstance(block.GetType(), new object[]{ block.FriendlyName, block.GVariable }) as BaseBlock;

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
    }
}
