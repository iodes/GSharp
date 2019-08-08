using GSharp.Base.Cores;
using GSharp.Base.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using GSharp.Common.Extensions;
using GSharp.Common.Objects;

namespace GSharp.Base.Objects.Customs
{
    public class GObjectCall : GObject, ICustomObject
    {
        #region Properties
        private IGCommand Command { get; }
        
        private IEnumerable<GObject> Arguments { get; }
        
        public Type CustomType => Command.ObjectType;
        #endregion

        #region Initializer
        public GObjectCall(IGCommand command)
        {
            Command = command;
        }

        public GObjectCall(IGCommand command, IEnumerable<GObject> arguments)
        {
            Command = command;
            Arguments = arguments;
        }
        #endregion

        public override string ToSource()
        {
            var argumentList = new List<string>();
            for (var i = 0; i < Command.Optionals?.Count(); i++)
            {
                argumentList.Add(i < Arguments?.Count()
                    ? GSharpUtils.CastParameterString(Arguments.ElementAt(i), Command.Optionals.ElementAt(i).ObjectType)
                    : "null");
            }

            return $"({Command.FullName}({string.Join(", ", argumentList)}))";
        }
    }
}
