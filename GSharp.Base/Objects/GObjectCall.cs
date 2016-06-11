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
        private GObject[] Arguments = null;
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
            var argumentList = new List<string>();
            for(int i=0; i<GCommand.Optionals?.Length; i++)
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

            return string.Format("({0}({1}))", GCommand.FullName, string.Join(", ", argumentList));
        }
    }
}
