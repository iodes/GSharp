using GSharp.Base.Cores;
using System;

namespace GSharp.Base.Objects.Lists
{
    public class GItem : GSettableObject
    {
        #region Properties
        private GObject Index { get; }
        
        private GObject Target { get; }

        public override Type SettableType => typeof(object);
        #endregion

        #region Initializer
        public GItem(GObject target, GObject index)
        {
            Index = index;
            Target = target;
        }
        #endregion

        public override string ToSource()
        {
            var target = Target?.ToSource() + (!(Target is GList) ? ".ToList()" : string.Empty);
            var index = Index?.ToSource() + (!(Index is GNumber) ? ".ToNumber()" : string.Empty);
            
            return $"{target}[{index}]";
        }
    }
}
