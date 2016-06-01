using System;
using System.Text;
using System.Collections.Generic;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;
using GSharp.Extension;
using GSharp.Base.Objects;

namespace GSharp.Base.Scopes
{
    [Serializable]
    public class GEvent : GScope
    {
        public GCommand GCommand { get; }
        public List<IVariable> Arguments { get; } = new List<IVariable>();

        public List<GStatement> Content = new List<GStatement>();

        public GEvent(GCommand command)
        {
            GCommand = command;

            for (int i = 0; i < GCommand.Optionals?.Length; i++)
            {
                string variableName = GCommand.Optionals[i].Name;
                variableName = "param" + variableName[0].ToString().ToUpper() + variableName.Substring(1);

                IVariable variable = GSharpUtils.CreateIVariable(variableName, GCommand.Optionals[i].ObjectType);
                Arguments.Add(variable);
            }
        }

        public GEvent(GCommand command, List<GStatement> content) : this(command)
        {
            Content = content;
        }

        public void Append(GStatement statement)
        {
            Content.Add(statement);
        }

        public void Clear()
        {
            Content.Clear();
        }

        public override string ToSource()
        {
            StringBuilder source = new StringBuilder();
            
            var argumentList = new List<string>();
            foreach (var arg in Arguments)
            {
                argumentList.Add(arg.Name);
            }

            string argumentsStr = string.Join(",", argumentList.ToArray());

            source.AppendFormat("{0}.{1} += ({2}) => \n{{\n", GCommand.NamespaceName, GCommand.MethodName, argumentsStr);

            foreach(GStatement statement in Content)
            {
                source.AppendLine(ConvertAssistant.Indentation(statement.ToSource()));
            }
            source.AppendLine("};");

            return source.ToString();
        }
    }
}
