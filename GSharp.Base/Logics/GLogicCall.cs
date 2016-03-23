using GSharp.Base.Cores;
using GSharp.Base.Scopes;
using GSharp.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Logics
{
    public class GLogicCall : GLogic
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
