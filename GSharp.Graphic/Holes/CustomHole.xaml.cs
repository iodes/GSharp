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
    public partial class CustomHole : BaseObjectHole
    {
        public override event HoleEventArgs BlockAttached;
        public override event HoleEventArgs BlockDetached;

        public Type Type
        {
            get
            {
                return _Type;
            }
        }
        private Type _Type;

        public override BaseBlock AttachedBlock
        {
            get
            {
                return CustomBlock;
            }
        }

        public override ObjectBlock BaseObjectBlock
        {
            get
            {
                return CustomBlock;
            }
        }

        public CustomVariableBlock CustomBlock
        {
            get
            {
                return BlockHole.Child as CustomVariableBlock;
            }

            set
            {
                // 제거는 DetachBlock으로
                if (value == null)
                {
                    DetachBlock();
                    return;
                }

                // 같은 블럭일 경우 무시
                if (value == BlockHole.Child) return;

                // 이미 블럭이 존재하는 경우
                if (BlockHole.Child != null)
                {
                    throw new Exception("이미 블럭이 존재합니다.");
                }

                if (value.Type != Type)
                {
                    throw new Exception("Type이 다른 블럭을 끼웠습니다.");
                }

                // 연결하려는 블럭을 부모에서 제거
                value?.ParentHole?.DetachBlock();

                // 블럭 연결
                value.ParentHole = this;
                BlockAttached?.Invoke(value);
                BlockHole.Child = value;
            }
        }

        public CustomHole(Type type, Color color)
        {
            InitializeComponent();
            _Type = type;

            color.R -= 20;
            color.G -= 20;
            color.B -= 20;

            Background = new SolidColorBrush(new Color() {
                R = color.R,
                G = color.G,
                B = color.B
            });
        }

        // 연결된 블럭을 제거
        public override BaseBlock DetachBlock()
        {
            var block = AttachedBlock;

            // Detach 이벤트 호출
            BlockDetached?.Invoke(block);

            // 블럭 연결 해제
            BlockHole.Child = null;

            return block;
        }
    }
}
