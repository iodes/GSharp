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

            List<string> args = new List<string>();
            string argStr = "";
            if (Command.Arguments != null)
            {
                for (int i = 0; i < Command.Arguments.Length; i++)
                {
                    args.Add("a" + i);
                }

                argStr = string.Join(",", args.ToArray());
            }

            source.AppendFormat("{0}.{1} += ({2}) => \n{{\n", Command.NamespaceName, Command.MethodName, argStr);

            foreach(GStatement statement in content)
            {
                source.AppendLine(ConvertAssistant.Indentation(statement.ToSource()));
            }
            source.AppendLine("};");

            return source.ToString();
        }
    }
}
