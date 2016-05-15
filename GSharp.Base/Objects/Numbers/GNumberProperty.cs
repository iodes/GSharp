using System;
using GSharp.Base.Cores;
using GSharp.Extension;

namespace GSharp.Base.Objects.Numbers
{
    [Serializable]
    public class GNumberProperty : GNumber, IProperty
    {
        #region 속성
        public GCommand GCommand { get; set; }
        #endregion

        #region 생성자
        public GNumberProperty(GCommand command)
        {
            GCommand = command;
        }
        #endregion

        public override string ToSource()
        {
            return GCommand.FullName;
        }
    }
}
