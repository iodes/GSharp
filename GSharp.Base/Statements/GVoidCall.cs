using System;
using GSharp.Base.Cores;
using System.Collections.Generic;
using GSharp.Base.Utilities;
using GSharp.Common.Extensions;
using System.Linq;

namespace GSharp.Base.Statements
{
    [Serializable]
    public class GVoidCall : GStatement
    {
        #region Properties
        private IGCommand Command { get; set; }

        public GObject[] Arguments { get; set; }
        #endregion

        #region Initializer
        public GVoidCall(IGCommand command)
        {
            Command = command;
        }

        public GVoidCall(IGCommand command, GObject[] arguments) : this(command)
        {
            Arguments = arguments;
        }
        #endregion

        public override string ToSource()
        {
            var argumentList = new List<string>();
            var optionalList = Command.Optionals.ToList();

            for (int i = 0; i < optionalList.Count; i++)
            {
                if (i < Arguments?.Length)
                {
                    argumentList.Add(GSharpUtils.CastParameterString(Arguments[i], optionalList[i].ObjectType));
                }
                else
                {
                    argumentList.Add("null");
                }
            }

            return string.Format("{0}({1});", Command.FullName, string.Join(", ", argumentList));
        }
    }
}
