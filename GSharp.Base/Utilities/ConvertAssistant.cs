using System;
using System.Text;
using GSharp.Base.Cores;
using GSharp.Base.Objects;

namespace GSharp.Base.Utilities
{
    public static class ConvertAssistant
    {
        public static string ResolveType(GBase value)
        {
            string result = string.Empty;

            if (value.GetType() == typeof(GVariable))
            {
                switch ((value as GVariable).CurrentType)
                {
                    case GVariable.VariableType.STRING:
                        result = "string";
                        break;

                    case GVariable.VariableType.NUMBER:
                        result = "long";
                        break;
                }
            }
            else if (value.GetType() == typeof(GString))
            {
                result = "string";
            }
            else if (value.GetType() == typeof(GNumber))
            {
                result = "long";
            }

            return result;
        }

        public static string Indentation(string value, int step = 1)
        {
            StringBuilder indent = new StringBuilder();
            for (int i = 0; i < step * 4; i++)
            {
                indent.Append(" ");
            }

            StringBuilder valueResult = new StringBuilder();
            string[] valueSplit = value.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < valueSplit.Length; i++)
            {
                if (valueSplit[i].Trim().Length > 0)
                {
                    valueResult.AppendLine(indent.ToString() + valueSplit[i]);
                }
            }

            return valueResult.ToString();
        }
    }
}
