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
        private const string PREFIX_REAL_ARG = "r_";
        private const string PREFIX_ARG = "param";

        public GCommand GCommand { get; }
        public List<GVariable> Arguments { get; } = new List<GVariable>();
        public List<Type> ArgumentsType { get; } = new List<Type>();

        public List<GStatement> Content = new List<GStatement>();

        public GEvent(GCommand command)
        {
            GCommand = command;

            for (int i = 0; i < GCommand.Optionals?.Length; i++)
            {
                string variableName = GCommand.Optionals[i].Name;
                variableName = PREFIX_ARG + variableName[0].ToString().ToUpper() + variableName.Substring(1);

                GVariable variable = GSharpUtils.CreateGVariable(variableName);

                Arguments.Add(variable);
                ArgumentsType.Add(GCommand.Optionals[i].ObjectType);
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
                argumentList.Add(PREFIX_REAL_ARG + arg.Name);
            }

            string argumentsStr = string.Join(",", argumentList.ToArray());

            source.AppendFormat("{0}.{1} += ({2}) => \n", GCommand.NamespaceName, GCommand.MethodName, argumentsStr);
            source.AppendLine("{");

            for (int i=0; i<Arguments.Count; i++)
            {
                source.AppendFormat("object {0} = {1}{0};\n", Arguments[i].Name, PREFIX_REAL_ARG);
            }

            foreach (GStatement statement in Content)
            {
                source.AppendLine(ConvertAssistant.Indentation(statement.ToSource()));
            }
            source.AppendLine("};");

            return source.ToString();
        }
    }
}
