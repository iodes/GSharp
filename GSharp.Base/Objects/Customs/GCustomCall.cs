using System;
using System.Linq;
using GSharp.Base.Cores;
using GSharp.Extension;
using GSharp.Base.Objects;

namespace GSharp.Base.Objects.Customs
{
    [Serializable]
    public class GCustomCall : GCustom, ICall
    {
        #region 객체
        public override Type CustomType
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
        private GObject[] targetArguments;
        #endregion

        #region 생성자
        public GCustomCall(GCommand valueMethod)
        {
            _GCommand = valueMethod;
        }

        public GCustomCall(GCommand valueMethod, GObject[] valueArguments)
        {
            _GCommand = valueMethod;
            targetArguments = valueArguments;
        }
        #endregion

        public override string ToCustomSource()
        {
            var argumentString = "";
            if (targetArguments != null)
            {
                argumentString = string.Join(", ", targetArguments.Select(element => element.ToSource()));
            }
            return string.Format("{0}({1})", GCommand.FullName, argumentString);
        }
    }
}
