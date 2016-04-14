using GSharp.Graphic.Holes;
using GSharp.Base.Cores;
using System.Collections.Generic;
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

        public BaseBlock()
        {
        }

        protected void Init()
        {
            if (HoleList == null) return;

            foreach (BaseHole hole in HoleList)
            {
                if (hole == null) continue;
                hole.BlockChanged += ObjectHole_HoleChanged;
            }
        }

        private void ObjectHole_HoleChanged(BaseBlock oldBlock, BaseBlock newBlock)
        {
            if (oldBlock != null)
            {
                foreach (BaseHole hole in oldBlock.HoleList)
                {
                    HoleList.Remove(hole);
                }

            }

            if (newBlock != null)
            {
                foreach (BaseHole hole in newBlock.HoleList)
                {
                    HoleList.Add(hole);
                }
            }
        }
    }
}
