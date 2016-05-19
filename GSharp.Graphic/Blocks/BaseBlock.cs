using GSharp.Graphic.Holes;
using GSharp.Base.Cores;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;
using GSharp.Graphic.Objects;

namespace GSharp.Graphic.Blocks
{
    public abstract class BaseBlock : UserControl
    {
        /// <summary>
        /// 자신의 하위 요소를 모두 포함하여 블럭에 붙어있는 구멍 목록
        /// </summary>
        public List<BaseHole> AllHoleList { get; } = new List<BaseHole>();

        /// <summary>
        /// 자신의 하위 요소를 제외하고 자신 블럭에 붙어있는 구멍 목록
        /// </summary>
        public virtual List<BaseHole> HoleList { get; } = new List<BaseHole>();
        
        /// <summary>
        /// 블럭에 붙일 수 있는 변수 목록
        /// </summary>
        public virtual List<IVariableBlock> AllowVariableList { get; } = new List<IVariableBlock>();


        /// <summary>
        /// 블럭을 GSharp Object로 변환
        /// </summary>
        public abstract List<GBase> ToGObjectList();

        // 블럭 위치
        /// <summary>
        /// 캔버스를 기준으로 하는 블럭의 위치
        /// </summary>
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
        /// <summary>
        /// 자신이 끼워져 있는 구멍
        /// null인 경우 구멍에 끼워지지 않음
        /// </summary>
        public BaseHole ParentHole { get; set; }

        /// <summary>
        /// 블럭을 초기화 하는 함수
        /// 모든 블럭은 생성자의 끝에 이 함수를 호출해야 한다.
        /// 이 함수에서 블럭의 모든 구멍에 이벤트를 추가하여 구멍에 블럭이 끼워지고 제거되는 것을 감지한다.
        /// </summary>
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

        /// <summary>
        /// 블럭이 끼워질 때의 이벤트
        /// 끼워진 블럭이 가지는 구멍들을 이 블럭이 가진 구멍 목록에 추가
        /// </summary>
        /// <param name="hole">블럭이 끼워진 구멍</param>
        /// <param name="block">구멍에 끼운 블럭</param>
        private void OnBlockAttached(BaseHole hole, BaseBlock block)
        {
            if (block != null)
            {
                foreach (var holeItem in block.HoleList)
                {
                    AllHoleList.Add(hole);
                }

                ParentHole?.ParentBlock?.OnBlockAttached(hole, block);
            }
        }

        /// <summary>
        /// 블럭이 떨어질 때의 이벤트
        /// 떨어진 블럭이 가지는 구멍들은 이 블럭이 가진 구멍 목록에서 제거
        /// </summary>
        /// <param name="hole">블럭이 떨어져 나온 구멍</param>
        /// <param name="block">구멍에서 떨어진 블럭</param>
        private void OnBlockDetached(BaseHole hole, BaseBlock block)
        {
            if (block != null)
            {
                foreach (var holeItem in block.HoleList)
                {
                    AllHoleList.Remove(hole);
                }

                ParentHole?.ParentBlock?.OnBlockDetached(hole, block);
            }
        }
    }
}
