using GSharp.Graphic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace GSharp.Graphic.Holes
{
    public abstract class BaseHole : UserControl
    {
        // 부모 블럭
        public BaseBlock ParentBlock { get; set; }

        // 연결된 블럭
        public abstract BaseBlock AttachedBlock { get; }
        
        public delegate void HoleEventArgs(BaseBlock block);

        // 블럭이 연결되었을 때 이벤트
        public virtual event HoleEventArgs BlockAttached;

        // 블럭이 제거되었을 때 이벤트
        public virtual event HoleEventArgs BlockDetached;

        // 블럭 연결을 해제
        public virtual BaseBlock DetachBlock()
        {
            return null;
        }

        private static Type[] numberTypes = new Type[] {
            typeof(Char),

            typeof(Int16),
            typeof(UInt16),

            typeof(Int32),
            typeof(UInt32),

            typeof(Int64),
            typeof(UInt64),

            typeof(Single),
            typeof(Double),
        };

        public static BaseHole CreateHole(Type holeType)
        {
            // StringHole
            if (holeType == typeof(string))
            {
                return new StringHole
                {
                    Foreground = new BrushConverter().ConvertFromString("#086748") as Brush
                };
            }

            // NumberHole
            if (numberTypes.Contains(holeType))
            {
                return new NumberHole();
            }

            return new CustomHole();
        }
    }
}
