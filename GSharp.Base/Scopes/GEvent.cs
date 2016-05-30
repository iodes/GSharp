using System;
using System.Text;
using System.Collections.Generic;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;
using GSharp.Extension;

namespace GSharp.Base.Scopes
{
    [Serializable]
    public class GEvent : GScope
    {
        public GCommand Command { get; set; }
        public List<GStatement> Content = new List<GStatement>();

        public GEvent(GCommand command)
        {
            Command = command;
        }

        public GEvent(GCommand command, List<GStatement> content) : this(command)
        {
            Content = content;
        }

        public void Append(GStatement statement)
        {
            Content.Add(statement);
        }

        public override string ToSource()
        {
            StringBuilder source = new StringBuilder();

            List<string> args = new List<string>();
            string argStr = "";
            if (Command.Optionals != null)
            {
                for (int i = 0; i < Command.Optionals.Length; i++)
                {
                    args.Add("a" + i);
                }

                argStr = string.Join(",", args.ToArray());
            }

            source.AppendFormat("{0}.{1} += ({2}) => \n{{\n", Command.NamespaceName, Command.MethodName, argStr);

            foreach(GStatement statement in Content)
            {
                source.AppendLine(ConvertAssistant.Indentation(statement.ToSource()));
            }
            source.AppendLine("};");

            return source.ToString();
        }
    }
}
