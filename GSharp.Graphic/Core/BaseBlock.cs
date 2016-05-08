using GSharp.Graphic.Holes;
using GSharp.Base.Cores;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;

namespace GSharp.Graphic.Core
{
    public abstract class BaseBlock : UserControl
    {
        // 블럭을 끼울 수 있는 구멍 목록 - 하위 블럭 포함
        public List<BaseHole> AllHoleList { get; } = new List<BaseHole>();

        // 블럭을 끼울 수 있는 구멍 목록 - 원래 자신의 블럭만
        public virtual List<BaseHole> HoleList { get; } = new List<BaseHole>();

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

        // 블럭이 붙어 있는 Hole, null인 경우 BlockEditor
        public BaseHole ParentHole
        {
            get
            {
                return _ParentHole;
            }
            set
            {
                _ParentHole = value;
            }
        }
        private BaseHole _ParentHole = null;

        // 생성자
        public BaseBlock()
        {
        }

        // 블럭의 모든 Hole에 BlockChanged 이벤트 핸들러 추가
        protected void InitializeBlock()
        {
            AllHoleList.AddRange(HoleList);

            foreach (BaseHole hole in HoleList)
            {
                hole.ParentBlock = this;
                hole.BlockAttached += OnBlockAttached;
                hole.BlockDetached += OnBlockDetached;
            }
        }

        // Hole의 블럭이 추가되면 그 블럭의 HoleList 추가
        private void OnBlockAttached(BaseBlock block)
        {
            if (block != null)
            {
                foreach (BaseHole hole in block.HoleList)
                {
                    AllHoleList.Add(hole);
                }

                ParentHole?.ParentBlock?.OnBlockAttached(block);
            }
        }

        // Hole에 블럭이 삭제되면 그 블럭의 HoleList도 제거
        private void OnBlockDetached(BaseBlock block)
        {
            if (block != null)
            {
                foreach (BaseHole hole in block.HoleList)
                {
                    AllHoleList.Remove(hole);
                }

                ParentHole?.ParentBlock?.OnBlockDetached(block);
            }
        }
    }
}
