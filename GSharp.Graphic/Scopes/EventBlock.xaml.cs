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
                return _GCommand;
            }
        }
        private GCommand _GCommand;

        public GEvent GEvent
        {
            get
            {
                List<GBase> content = NextConnectHole?.StatementBlock?.ToGObjectList();
                if (content == null)
                {
                    content = new List<GBase>();
                }

                GEvent evt = new GEvent(GCommand);

                foreach (GBase gbase in content)
                {
                    if (gbase is GStatement)
                    {
                        evt.Append(gbase as GStatement);
                    }
                }

                return evt;
            }
        }

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

            _GCommand = command;
            StackContentText.Text = command.FriendlyName;

            // Initialize Hole List
            HoleList.Add(NextConnectHole);
            
            for (int i=0; i<command.Arguments.Length; i++)
            {
                AllowVariableList.Add(BlockUtils.CreateParameterVariable(command.Arguments[i], command.Arguments[i].ToString()));
            }

            // Initialize Block
            InitializeBlock();
        }
    }
}
