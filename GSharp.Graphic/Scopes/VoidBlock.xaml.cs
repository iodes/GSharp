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
using GSharp.Base;

namespace GSharp.Graphic.Scopes
{
    /// <summary>
    /// MethodBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class VoidBlock : ScopeBlock
    {
        public VoidBlock()
        {
            InitializeComponent();
        }

        public override List<GBase> GObjectList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override GScope GScope
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
