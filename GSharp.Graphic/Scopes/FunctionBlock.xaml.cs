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
    public partial class FunctionBlock : ScopeBlock
    {
        public string FunctionName;

        public FunctionBlock(string name)
        {
            InitializeComponent();
            FunctionName = name;
            StackContentText.Text = name;
            InitializeBlock();
        }

        public GFunction GFunction
        {
            get
            {
                List<GBase> content = NextConnectHole?.StatementBlock?.GObjectList;
                if (content == null)
                {
                    content = new List<GBase>();
                }

                GFunction func = new GFunction(FunctionName);

                foreach (GBase gbase in content)
                {
                    if (gbase is GStatement)
                    {
                        func.Append(gbase as GStatement);
                    }
                }

                return func;
            }
        }

        public override GScope GScope
        {
            get
            {
                return GFunction;
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
