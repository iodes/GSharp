using GSharp.Base.Cores;
using GSharp.Graphic.Core;
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
    /// ObjectHole.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ObjectHole : BaseObjectHole
    {
        public override event HoleEventArgs BlockChanged;

        public override BaseBlock Block
        {
            get
            {
                return ObjectBlock;
            }
        }
        
        public override ObjectBlock BaseObjectBlock
        {
            get
            {
                return ObjectBlock;
            }
        }

        public ObjectBlock ObjectBlock
        {
            get
            {
                return (ObjectBlock)RealObjectBlock.Child;
            }
            set
            {
                if (value == RealObjectBlock.Child) return;

                if (RealObjectBlock.Child != null)
                {
                    throw new Exception("이미 블럭이 존재합니다.");
                }

                BlockChanged?.Invoke(Block, value);
                RealObjectBlock.Child = value;
            }
        }

        public ObjectHole()
        {
            InitializeComponent();
        }
    }
}
