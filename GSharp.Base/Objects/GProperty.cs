using System;
using GSharp.Base.Cores;
using GSharp.Extension;

namespace GSharp.Base.Objects
{
    [Serializable]
    public class GProperty : GObject
    {
        #region 속성
        public GCommand GCommand { get; set; }
        #endregion

        #region 생성자
        public GProperty(GCommand command)
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
