using GSharp.Base.Cores;
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

        public static string CastParameterString(GObject obj, Type type)
        {
            string castString = "";
            if (IsNumberType(type))
            {
                if (obj is GNumber)
                {
                    if (type != typeof(double))
                    {
                        return string.Format("({0}){1}", castString, obj.ToSource());
                    }

                    return obj.ToSource();
                }
                else
                {
                    if (type != typeof(double))
                    {
                        return string.Format("({0}){1}.ToNumber()", castString, obj.ToSource());
                    }

                    return string.Format("{0}.ToNumber()", obj.ToSource());
                }
            }

            if (type == typeof(string))
            {
                if (obj is GString)
                {
                    return obj.ToSource();
                }

                return string.Format("{0}.ToText()", obj.ToSource());
            }

            if (type == typeof(bool))
            {
                if (obj is GLogic)
                {
                    return obj.ToSource();
                }

                return string.Format("{0}.ToBool()", obj.ToSource());
            }

            // is List
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)))
            {
                Type listType = type.GetGenericArguments()[0];
                if (obj is GList)
                {
                    return string.Format("{0}.ToList<{1}>()", obj.ToSource(), listType.ToString());
                }

                return string.Format("{0}.ToList().ToList<{1}>()", obj.ToSource(), listType.ToString());
            }

            return string.Format("({0}){1}", type.ToString(), obj.ToSource());
        }

        public static bool IsNumberType(Type type)
        {
            if (numberTypes.Contains(type))
            {
                return true;
            }

            return false;
        }
    }
}
