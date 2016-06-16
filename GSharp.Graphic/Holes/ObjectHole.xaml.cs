using GSharp.Base.Cores;
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Objects;
using GSharp.Graphic.Objects.Numbers;
using GSharp.Graphic.Objects.Strings;
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
        public override event HoleEventArgs BlockAttached;
        public override event HoleEventArgs BlockDetached;

        public override BaseBlock AttachedBlock
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
            set
            {
                ObjectBlock = value;
            }
        }

        public ObjectBlock ObjectBlock
        {
            get
            {
                if (BlockHole.Child != null)
                {
                    return BlockHole.Child as ObjectBlock;
                }
                else
                {
                    double result;
                    if (double.TryParse(ConstText.Text, out result))
                    {
                        return new NumberConstBlock(result);
                    }
                    else
                    {
                        return new StringConstBlock(ConstText.Text);
                    }
                }
            }
            set
            {
                var prevBlock = BlockHole.Child;

                // 제거는 DetachBlock으로
                if (value == null)
                {
                    DetachBlock();
                    return;
                }

                // 같은 블럭일 경우 무시
                if (value == prevBlock) return;

                // 이미 블럭이 존재하는 경우
                if (prevBlock != null)
                {
                    throw new Exception("이미 블럭이 존재합니다.");
                }

                if (value is NumberConstBlock)
                {
                    ConstText.Text = (value as NumberConstBlock).Number.ToString();
                    return;
                }

                if (value is StringConstBlock)
                {
                    ConstText.Text = (value as StringConstBlock).String;
                    return;
                }

                // 연결하려는 블럭을 부모에서 제거
                value?.ParentHole?.DetachBlock();
                
                // 블럭 연결
                value.ParentHole = this;
                BlockAttached?.Invoke(this, value);
                BlockHole.Child = value;

                ConstText.Visibility = Visibility.Hidden;
            }
        }

        public ObjectHole()
        {
            InitializeComponent();
        }

        // 연결된 블럭을 제거
        public override BaseBlock DetachBlock()
        {
            var block = AttachedBlock;

            // Detach 이벤트 호출
            BlockDetached?.Invoke(this, block);

            // 블럭 연결 해제
            BlockHole.Child = null;

            ConstText.Visibility = Visibility.Visible;

            return block;
        }

        public override bool CanAttachBlock(BaseBlock block)
        {
            if (!(block is ObjectBlock))
            {
                return false;
            }

            return base.CanAttachBlock(block);
        }

        protected override void DisableHole()
        {
            ConstText.IsEnabled = false;
        }
    }
}
