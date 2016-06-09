using System;
using System.Linq;
using GSharp.Base.Cores;
using GSharp.Extension;
using GSharp.Base.Objects;

namespace GSharp.Base.Objects.Logics
{
    [Serializable]
    public class GLogicCall : GLogic, ICall
    {
        #region 객체
        private GCommand targetCommand;
        private GObject[] targetArguments;
        #endregion

        #region 생성자
        public GLogicCall(GCommand valueMethod)
        {
            targetCommand = valueMethod;
        }

        public GLogicCall(GCommand valueMethod, GObject[] valueArguments)
        {
            targetCommand = valueMethod;
            targetArguments = valueArguments;
        }
        #endregion

        public override string ToLogicSource()
        {
            if (targetArguments == null)
            {
                return string.Format
                (
                    "{0}()",
                    targetCommand.FullName
                );
            }
            else
            {
                var argumentStrings = targetArguments.Select(element => element.ToSource());
            
                return string.Format
                (
                    "{0}({1})",
                    targetCommand.FullName,
                    string.Join(", ", argumentStrings)
                );
            }
        }
    }
}
