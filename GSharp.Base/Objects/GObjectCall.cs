using System;
using System.Linq;
using GSharp.Base.Cores;
using GSharp.Extension;

namespace GSharp.Base.Objects
{
    [Serializable]
    public class GObjectCall : GObject
    {
        #region 객체
        private GCommand targetCommand;
        private GObject[] targetArguments;
        #endregion

        #region 생성자
        public GObjectCall(GCommand valueMethod)
        {
            targetCommand = valueMethod;
        }

        public GObjectCall(GCommand valueMethod, GObject[] valueArguments)
        {
            targetCommand = valueMethod;
            targetArguments = valueArguments;
        }
        #endregion

        public override string ToSource()
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
