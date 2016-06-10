using GSharp.Base.Cores;
using GSharp.Base.Utilities;
using GSharp.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects
{
    public class GObjectCall : GObject
    {
        #region 객체
        private GCommand GCommand;
        private GObject[] Arguments;
        #endregion

        #region 생성자
        public GObjectCall(GCommand command)
        {
            GCommand = command;
        }

        public GObjectCall(GCommand command, GObject[] arguments)
        {
            GCommand = command;
            Arguments = arguments;
        }
        #endregion

        public override string ToSource()
        {
            string argumentStrings = "";
            if (Arguments != null)
            {
                argumentStrings = string.Join(", ", Arguments.Select(element => element.ToSource()));
            }

            return string.Format("(({0}){1}({2}))", GSharpUtils.GetCastString(GCommand.ObjectType), GCommand.FullName, argumentStrings);
        }
    }
}
