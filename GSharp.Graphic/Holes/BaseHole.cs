using GSharp.Graphic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        public static BaseHole CreateHole(string holeName)
        {   
            switch (holeName)
            {
                case "logic":
                    return new LogicHole();

                case "object":
                    return new ObjectHole();

                case "variable":
                    return new VariableHole();

                default:
                    return null;
            }
        }
    }
}
