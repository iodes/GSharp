using System;
using GSharp.Base.Cores;
using GSharp.Extension;

namespace GSharp.Base.Objects.Customs
{
    [Serializable]
    public class GCustomProperty : GCustom, IProperty
    {
        #region 속성
        public override Type Type
        {
            get
            {
                return GCommand.ObjectType;
            }
        }

        public GCommand GCommand
        {
            get
            {
                return _GCommand;
            }
        }
        private GCommand _GCommand;
        #endregion

        #region 생성자
        public GCustomProperty(GCommand command)
        {
            _GCommand = command;
        }
        #endregion

        public override string ToSource()
        {
            return GCommand.FullName;
        }
    }
}
