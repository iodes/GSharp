using System.Linq;
using GSharp.Base.Cores;
using GSharp.Base.Scopes;

namespace GSharp.Base.Statements
{
    public class GCall : GStatement
    {
        #region 객체
        private GVoid targetVoid;
        private GMethod targetMethod;
        private GObject[] targetArguments;
        #endregion

        #region 생성자
        public GCall(GVoid valueVoid, GObject[] valueArguments)
        {
            targetVoid = valueVoid;
            targetArguments = valueArguments;
        }

        public GCall(GMethod valueMethod, GObject[] valueArguments)
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
                    "{0}({1});",
                    targetVoid.Name,
                    string.Join(",", argumentStrings)
                );
            }
            else
            {
                return string.Format
                (
                    "{0}({1});",
                    targetMethod.ToSource(),
                    string.Join(",", argumentStrings)
                );
            }
        }
    }
}
