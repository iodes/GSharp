using System;
using GSharp.Base.Cores;
using GSharp.Extension;

namespace GSharp.Base.Objects
{
    [Serializable]
    public class GLogicProperty : GLogic
    {
        #region 속성
        public GCommand GCommand { get; set; }
        #endregion

        #region 생성자
        public GLogicProperty(GCommand command)
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
