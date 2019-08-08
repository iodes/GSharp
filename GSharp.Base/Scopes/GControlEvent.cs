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

        public string ControlName { get; }
        public GExport GExport { get; }
        public List<GVariable> Arguments { get; } = new List<GVariable>();
        public List<Type> ArgumentsType { get; } = new List<Type>();

        public List<GStatement> Content = new List<GStatement>();

        public GControlEvent(string controlName, GExport export)
        {
            ControlName = controlName;
            GExport = export;

            for (int i = 0; i < GExport.Optionals?.Length; i++)
            {
                string variableName = GExport.Optionals[i].Name;
                variableName = PREFIX_ARG + variableName[0].ToString().ToUpper() + variableName.Substring(1);

                GVariable variable = GSharpUtils.CreateGVariable(variableName);

                Arguments.Add(variable);
                ArgumentsType.Add(GExport.Optionals[i].ObjectType);
            }
        }

        public GControlEvent(string controlName, GExport export, List<GStatement> content) : this(controlName, export)
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

            source.AppendFormat($@"FindControl<{GExport.NamespaceName}>(window, ""{ControlName}"").{GExport.MethodName} += () =>");
            source.AppendLine("\n{");
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
