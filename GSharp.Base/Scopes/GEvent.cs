using System;
using System.Text;
using System.Collections.Generic;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;
using GSharp.Base.Objects;
using GSharp.Common.Extensions;

namespace GSharp.Base.Scopes
{
    [Serializable]
    public class GEvent : GScope
    {
        #region Constants
        private const string PREFIX_REAL_ARG = "r_";
        private const string PREFIX_ARG = "param";
        #endregion

        #region Properties
        public IGCommand Command { get; }

        public List<GVariable> Arguments { get; } = new List<GVariable>();

        public List<Type> ArgumentsType { get; } = new List<Type>();

        public List<GStatement> Content = new List<GStatement>();
        #endregion

        #region Initializer
        public GEvent(IGCommand command)
        {
            Command = command;

            foreach (var optional in Command.Optionals)
            {
                var variableName = PREFIX_ARG + optional.Name[0].ToString().ToUpper() + optional.Name.Substring(1);
                var variable = GSharpUtils.CreateGVariable(variableName);

                Arguments.Add(variable);
                ArgumentsType.Add(optional.ObjectType);
            }
        }

        public GEvent(IGCommand command, List<GStatement> content) : this(command)
        {
            Content = content;
        }
        #endregion

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
            var source = new StringBuilder();
            
            var argumentList = new List<string>();
            foreach (var arg in Arguments)
            {
                argumentList.Add(PREFIX_REAL_ARG + arg.Name);
            }

            var argumentsStr = string.Join(",", argumentList.ToArray());

            source.AppendFormat("{0}.{1} += ({2}) => \n", Command.NamespaceName, Command.MethodName, argumentsStr);
            source.AppendLine("{");
            source.AppendLine("    Dispatcher.Invoke(() =>");
            source.AppendLine("    {");

            for (int i = 0; i < Arguments.Count; i++)
            {
                source.AppendFormat("object {0} = {1}{0};\n", Arguments[i].Name, PREFIX_REAL_ARG);
            }

            foreach (GStatement statement in Content)
            {
                source.AppendLine(ConvertAssistant.Indentation(statement.ToSource(), 2));
            }

            source.AppendLine("    });");
            source.AppendLine("};");

            return source.ToString();
        }
    }
}
