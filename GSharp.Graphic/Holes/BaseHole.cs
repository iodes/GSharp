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
using System.Xml;

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

        public bool IsPreview
        {
            set
            {
                DisableHole();
            }
        }

        protected virtual void DisableHole()
        {

        }

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

                if (!globalVariableList.Any(e => e.GVariable == varBlock.GVariable) && !allowVariableList.Any(e => e.GVariable == varBlock.GVariable))
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


        public void SaveXML(XmlWriter writer)
        {
            writer.WriteStartElement("Hole");
            writer.WriteAttributeString("HoleType", GetType().ToString());

            SaveHoleAttribute(writer);
            AttachedBlock?.SaveXML(writer);

            writer.WriteEndElement();
        }

        protected virtual void SaveHoleAttribute(XmlWriter writer) { }
    }
}
