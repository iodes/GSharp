using GSharp.Base.Objects;
using GSharp.Base.Objects.Customs;
using GSharp.Base.Objects.Logics;
using GSharp.Base.Objects.Numbers;
using GSharp.Base.Objects.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Utilities
{
    public static class GSharpUtils
    {
        public static Type[] numberTypes = {
            typeof(sbyte), // SByte
            typeof(byte), // Byte
            typeof(short), // Int16
            typeof(ushort), // UInt16
            typeof(int), // Int32
            typeof(uint), // UInt32
            typeof(long), // Int64
            typeof(ulong), // UInt64
            typeof(float), // Single
            typeof(double), // Double
        };

        public static GVariable CreateGVariable(string variableName)
        {
            if (variableName?.Length <= 0) return null;

            return new GVariable(variableName);
        }

        public static string GetCastString(Type type)
        {
            if (type == typeof(string))
            {
                return "GString";
            }

            if (type == typeof(bool))
            {
                return "GLogic";
            }

            if (numberTypes.Contains(type))
            {
                return "GNumber";
            }

            return "GCustom";
        }
    }
}
