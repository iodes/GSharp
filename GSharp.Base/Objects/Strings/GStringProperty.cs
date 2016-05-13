using System;
using GSharp.Base.Cores;
using GSharp.Extension;

namespace GSharp.Base.Objects.Strings
{
    [Serializable]
    public class GStringProperty : GString, IProperty
    {
        #region 속성
        public GCommand GCommand { get; set; }
        #endregion

        #region 생성자
        public GStringProperty(GCommand command)
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
