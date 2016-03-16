using GSharp.Base.Cores;
using GSharp.Base.Scopes;
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
        private GVoid targetVoid;
        private GMethod targetMethod;
        private GObject[] targetArguments;
        #endregion

        #region 생성자
        public GLogicCall(GVoid valueVoid, GObject[] valueArguments)
        {
            targetVoid = valueVoid;
            targetArguments = valueArguments;
        }

        public GLogicCall(GMethod valueMethod, GObject[] valueArguments)
        {
            targetMethod = valueMethod;
            targetArguments = valueArguments;
        }
        #endregion

        public override string ToSource()
        {
            var argumentStrings = targetArguments.Select(element => element.ToSource());

            if (targetVoid != null)
            {
                return string.Format
                (
                    "{0}({1})",
                    targetVoid.Name,
                    string.Join(",", argumentStrings)
                );
            }
            else
            {
                return string.Format
                (
                    "{0}({1})",
                    targetMethod.ToSource(),
                    string.Join(",", argumentStrings)
                );
            }
        }
    }
}
