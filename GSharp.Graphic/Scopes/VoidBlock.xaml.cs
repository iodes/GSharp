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
using GSharp.Cores;
using GSharp.Scopes;

namespace GSharp.Graphic.Scopes
{
    /// <summary>
    /// MethodBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class VoidBlock : ScopeBlock
    {
        public string MethodName { get; set; } = "Main";

        public VoidBlock()
        {
            InitializeComponent();
        }

        public override List<BaseHole> GetHoleList()
        {
            List<BaseHole> baseHoleList = new List<BaseHole>();

            baseHoleList.Add(NextConnectHole);

            return baseHoleList;
        }

        public override List<GBase> ToObject()
        {
            List<GBase> baseList = new List<GBase>();
            GVoid gVoid = new GVoid(MethodName);

            List<GBase> nextList = NextConnectHole?.StatementBlock?.ToObject();
            if (nextList != null)
            {
                foreach (GBase next in nextList)
                {
                    GStatement nextStatement = (GStatement)next;
                    gVoid.Append(nextStatement);
                }
            }

            baseList.Add(gVoid);

            return baseList;
        }
    }
}
