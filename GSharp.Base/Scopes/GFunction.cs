using System.Text;
using System.Collections.Generic;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;
using System;

namespace GSharp.Base.Scopes
{
    public class GFunction : GScope
    {
        public string FunctionName { get; set; }
        public List<GStatement> Content = new List<GStatement>();

        public GFunction(string name)
        {
            FunctionName = name;
        }

        public GFunction(string name, List<GStatement> content) : this(name)
        {
            Content = content;
        }

        public void Append(GStatement statement)
        {
            Content.Add(statement);
        }

        public override string ToSource()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("void {0}()\n{{\n", FunctionName);

            foreach (GStatement statement in Content)
            {
                builder.AppendLine(ConvertAssistant.Indentation(statement.ToSource()));
            }

            builder.AppendLine("};");

            return builder.ToString();
        }
    }
}