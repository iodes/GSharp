using GSharp.Graphic.Core;
using GSharp.Graphic.Objects;
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

namespace GSharp.Graphic.Holes
{
    /// <summary>
    /// VariableHole.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NumberHole : BaseObjectHole
    {
        public override BaseBlock AttachedBlock
        {
            get
            {
                return NumberBlock;
            }
        }

        public override ObjectBlock BaseObjectBlock
        {
            get
            {
                return NumberBlock;
            }
        }

        public NumberBlock NumberBlock
        {
            get
            {
                return new NumberBlock
                {
                    Number = Number
                };
            }
        }

        public long Number { get; set; } = 0;

        public NumberHole()
        {
            InitializeComponent();
        }
    }
}
