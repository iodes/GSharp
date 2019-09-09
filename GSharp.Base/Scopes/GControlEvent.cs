using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Base.Utilities;
using GSharp.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GSharp.Base.Scopes
{
    [Serializable]
    public class GControlEvent : GScope
    {
        #region Constants
        private const string PREFIX_REAL_ARG = "r_";
        private const string PREFIX_ARG = "param";
        #endregion

        #region Properties
        public string ControlName { get; }

        public IGExportedData ExportedData { get; }

        public List<GVariable> Arguments { get; } = new List<GVariable>();

        public List<Type> ArgumentsType { get; } = new List<Type>();

        public List<GStatement> Content = new List<GStatement>();
        #endregion

        #region Initializer
        public GControlEvent(string controlName, IGExportedData exportedData)
        {
            ControlName = controlName;
            ExportedData = exportedData;

            foreach (var optional in ExportedData.Optionals)
            {
                var variableName = PREFIX_ARG + optional.Name[0].ToString().ToUpper() + optional.Name.Substring(1);
                var variable = GSharpUtils.CreateGVariable(variableName);

                Arguments.Add(variable);
                ArgumentsType.Add(optional.ObjectType);
            }
        }

        public GControlEvent(string controlName, IGExportedData exportedData, List<GStatement> content) : this(controlName, exportedData)
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

            source.AppendFormat($@"FindControl<{ExportedData.NamespaceName}>(window, ""{ControlName}"").{ExportedData.MethodName} += () =>");
            source.AppendLine("\n{");
            source.AppendLine("    Dispatcher.Invoke(() =>");
            source.AppendLine("    {");

            for (var i = 0; i < Arguments.Count; i++)
            {
                source.AppendFormat("object {0} = {1}{0};\n", Arguments[i].Name, PREFIX_REAL_ARG);
            }

            foreach (var statement in Content)
            {
                source.AppendLine(ConvertAssistant.Indentation(statement.ToSource(), 2));
            }

            source.AppendLine("    });");
            source.AppendLine("};");

            return source.ToString();
        }
    }
}
