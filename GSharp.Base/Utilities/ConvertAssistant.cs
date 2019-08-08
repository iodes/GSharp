using System;
using System.Text;
using GSharp.Base.Cores;
using GSharp.Base.Objects;

namespace GSharp.Base.Utilities
{
    public static class ConvertAssistant
    {
        public static string Indentation(string value, int step = 1)
        {
            if (value == null) return null;
            
            var indent = new StringBuilder();
            for (var i = 0; i < step * 4; i++)
            {
                indent.Append(" ");
            }

            var results = new StringBuilder();
            foreach (var t in value.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None))
            {
                if (t.Trim().Length > 0)
                {
                    results.AppendLine(indent + t);
                }
            }

            return results?.ToString();
        }
    }
}
