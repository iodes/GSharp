using GSharp.Base.Objects;
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace GSharp.Graphic.Holes
{
    /// <summary>
    /// 모든 구멍들의 최상위 클래스로써 구멍에 필수적으로 필요한 내용을 정의
    /// </summary>
    public abstract class BaseHole : UserControl
    {
        /// <summary>
        /// 구멍이 붙어있는 블럭입니다.
        /// </summary>
        public BaseBlock ParentBlock { get; set; }

        /// <summary>
        /// 구멍에 끼워져 있는 블럭입니다.
        /// </summary>
        public abstract BaseBlock AttachedBlock { get; }

        /// <summary>
        /// 구멍에 블럭이 끼워지거나 떨어지는 이벤트의 대리자입니다.
        /// </summary>
        /// <param name="hole">이벤트가 발생한 구멍</param>
        /// <param name="block">이벤트가 발생한 블럭</param>
        public delegate void HoleEventArgs(BaseHole hole, BaseBlock block);


        /// <summary>
        /// 구멍에 블럭이 끼워졌을 때 이벤트입니다.
        /// </summary>
        public virtual event HoleEventArgs BlockAttached;

        /// <summary>
        /// 구멍에 블럭이 떨어졌을 때 이벤트입니다.
        /// </summary>
        public virtual event HoleEventArgs BlockDetached;

        /// <summary>
        /// 구멍에 해당 블럭을 끼울 수 있는지 확인합니다.
        /// </summary>
        /// <param name="block">확인하고 싶은 블럭</param>
        /// <returns>
        /// <c>true</c>: 끼울 수 있을 때
        /// <c>false</c>: 끼울 수 없을 때
        /// </returns>
        public virtual bool CanAttachBlock(BaseBlock block)
        {
            if (block.AllHoleList.Contains(this))
            {
                return false;
            }
            
            if (block is VariableBlock)
            {
                var varBlock = block as VariableBlock;

                var globalVariableList = block.BlockEditor.GetGlobalVariableBlockList();
                var allowVariableList = ParentBlock.AllowVariableList;

                if (!globalVariableList.Contains(varBlock) && !allowVariableList.Contains(varBlock))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 끼워져 있는 블럭을 떨어트립니다.
        /// </summary>
        /// <returns>떨어진 블럭을 반환</returns>
        public abstract BaseBlock DetachBlock();

        /// <summary>
        /// Hole에 끼인 블럭의 소스를 반환합니다.
        /// Hole의 특성에 따라 조작될 수 있습니다.
        /// </summary>
        /// <returns></returns>
        public virtual string ToSource()
        {
            return AttachedBlock?.ToGObjectList()[0]?.ToSource();
        }
    }
}
