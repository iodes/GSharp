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
using GSharp.Base.Scopes;
using GSharp.Extension;

namespace GSharp.Graphic.Scopes
{
    /// <summary>
    /// MethodBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EventBlock : ScopeBlock
    {
        public GCommand Command { get; set; }

        public EventBlock(GCommand command)
        {
            InitializeComponent();
            Command = command;
            StackContentText.Text = command.FriendlyName;
        }

        public GEvent GEvent
        {
            get
            {
                List<GBase> content = NextConnectHole?.StatementBlock?.GObjectList;
                if (content == null)
                {
                    content = new List<GBase>();
                }

                GEvent evt = new GEvent(Command);

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

        public override List<GBase> GObjectList
        {
            get
            {
                return new List<GBase> { GScope };
            }
        }

        public override List<BaseHole> HoleList
        {
            get
            {
                return new List<BaseHole> { NextConnectHole };
            }
        }
    }
}
