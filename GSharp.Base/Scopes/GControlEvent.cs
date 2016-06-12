using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Base.Utilities;
using GSharp.Extension;
using GSharp.Extension.Exports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Scopes
{
    [Serializable]
    public class GControlEvent : GScope
    {
        private const string PREFIX_REAL_ARG = "r_";
        private const string PREFIX_ARG = "param";

        public GControl GControl { get; }
        public GExport GExport { get; }
        public List<GVariable> Arguments { get; } = new List<GVariable>();
        public List<Type> ArgumentsType { get; } = new List<Type>();

        public List<GStatement> Content = new List<GStatement>();

        public GControlEvent(GControl control, GExport export)
        {
            GControl = control;
            GExport = export;

            if (!GControl.Exports.Contains(GExport))
            {
                throw new Exception("Not allowed event");
            }

            for (int i = 0; i < GExport.Optionals?.Length; i++)
            {
                string variableName = GExport.Optionals[i].Name;
                variableName = PREFIX_ARG + variableName[0].ToString().ToUpper() + variableName.Substring(1);

                GVariable variable = GSharpUtils.CreateGVariable(variableName);

                Arguments.Add(variable);
                ArgumentsType.Add(GExport.Optionals[i].ObjectType);
            }
        }

        public GControlEvent(GControl control, GExport export, List<GStatement> content) : this(control, export)
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
            var source = new StringBuilder();

            var argumentList = new List<string>();
            foreach (var arg in Arguments)
            {
                argumentList.Add(PREFIX_REAL_ARG + arg.Name);
            }

            string argumentsStr = string.Join(",", argumentList.ToArray());

            source.AppendFormat("FindControl(window, \"{0}\").{1} += () => \n", GControl.NamespaceName, GExport.MethodName);
            source.AppendLine("{");

            for (int i = 0; i < Arguments.Count; i++)
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
