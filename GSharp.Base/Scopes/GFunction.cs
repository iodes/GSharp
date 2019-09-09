using System;
using System.Text;
using System.Collections.Generic;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;

namespace GSharp.Base.Scopes
{
    [Serializable]
    public class GFunction : GScope
    {
        #region Properties
        public string FunctionName { get; set; }

        public List<GStatement> Content = new List<GStatement>();
        #endregion

        #region Initializer
        public GFunction(string name)
        {
            FunctionName = name;
        }

        public GFunction(string name, List<GStatement> content) : this(name)
        {
            Content = content;
        }
        #endregion

        public void Append(GStatement statement)
        {
            Content.Add(statement);
        }

        public override string ToSource()
        {
            var builder = new StringBuilder();
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
