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
        public GCall(GVoid valueVoid)
        {
            targetVoid = valueVoid;
        }

        public GCall(GVoid valueVoid, GObject[] valueArguments) : this(valueVoid)
        {
            targetArguments = valueArguments;
        }

        public GCall(GMethod valueMethod)
        {
            targetMethod = valueMethod;
        }

        public GCall(GMethod valueMethod, GObject[] valueArguments) : this(valueMethod)
        {
            targetArguments = valueArguments;
        }
        #endregion

        public override string ToSource()
        {
            string valueTarget = targetVoid == null ? targetMethod.ToSource() : targetVoid.Name;

            if (targetArguments == null)
            {
                return string.Format
                (
                    "{0}();",
                    valueTarget
                );
            }
            else
            {
                var argumentStrings = targetArguments.Select(element => element.ToSource());

                return string.Format
                (
                    "{0}({1});",
                    valueTarget,
                    string.Join(", ", argumentStrings)
                );
            }
        }
    }
}
