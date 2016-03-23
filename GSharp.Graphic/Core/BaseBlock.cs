using GSharp.Graphic.Holes;
using GSharp.Base.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GSharp.Graphic.Core
{
    public abstract class BaseBlock : UserControl
    {
        // 블럭을 끼울 수 있는 구멍 목록
        public virtual List<BaseHole> HoleList
        {
            get
            {
                return new List<BaseHole>();
            }
        }

        // GSharp Object로 변환된 블럭
        public abstract List<GBase> GObjectList { get; }

        // 블럭 위치
        public Point Position
        {
            get
            {
                return new Point(Margin.Left, Margin.Top);
            }
            set
            {
                Margin = new Thickness(value.X, value.Y, 0, 0);
            }
        }
    }
}
