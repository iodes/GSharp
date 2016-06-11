using System;
using System.Linq;
using GSharp.Base.Cores;
using GSharp.Extension;
using GSharp.Base.Scopes;
using System.Collections.Generic;
using GSharp.Base.Utilities;

namespace GSharp.Base.Statements
{
    [Serializable]
    public class GVoidCall : GStatement
    {
        #region 객체
        private GCommand GCommand;
        public GObject[] Arguments;
        #endregion

        #region 생성자
        public GVoidCall(GCommand command)
        {
            GCommand = command;
        }

        public GVoidCall(GCommand command, GObject[] arguments) : this(command)
        {
            Arguments = arguments;
        }
        #endregion

        public override string ToSource()
        {
            var argumentList = new List<string>();
            for (int i = 0; i < GCommand.Optionals?.Length; i++)
            {
                if (i < Arguments?.Length)
                {
                    argumentList.Add(GSharpUtils.CastParameterString(Arguments[i], GCommand.Optionals[i].ObjectType));
                }
                else
                {
                    argumentList.Add("null");
                }
            }

            return string.Format("{0}({1});", GCommand.FullName, string.Join(", ", argumentList));
        }
    }
}
