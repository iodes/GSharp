using System.Text;
using System.Collections.Generic;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;
using GSharp.Extension;
using System;

namespace GSharp.Base.Scopes
{
    public class GEvent : GScope
    {
        public GCommand Command { get; set; }
        public List<GStatement> content = new List<GStatement>();

        public GEvent(GCommand command)
        {
            Command = command;
        }

        public GEvent(GCommand command, List<GStatement> content) : this(command)
        {
            this.content = content;
        }

        public void Append(GStatement statement)
        {
            content.Add(statement);
        }

        public override string ToSource()
        {
            StringBuilder source = new StringBuilder();
            source.AppendFormat("{0}.{1} += () => {{\n", Command.NamespaceName, Command.MethodName);

            foreach(GStatement statement in content)
            {
                source.AppendLine(statement.ToSource());
            }

            source.AppendLine("};");

            return source.ToString();
        }
    }
}
