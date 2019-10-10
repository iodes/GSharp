using GSharp.Base.Cores;
using GSharp.Base.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using GSharp.Common.Objects;

namespace GSharp.Base.Utilities
{
    public static class GSharpUtils
    {
        private static readonly Type[] NumberTypes =
        {
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort)
        };

        public static string GetTypeName(Type t)
        {
            if (!t.IsGenericType) return t.Name;
            if (t.DeclaringType != null && t.IsNested && t.DeclaringType.IsGenericType)
                throw new NotImplementedException();

            var txt = t.Name.Substring(0, t.Name.IndexOf('`')) + "<";
            var cnt = 0;

            foreach (var arg in t.GetGenericArguments())
            {
                if (cnt > 0) txt += ", ";
                txt += GetTypeName(arg);
                cnt++;
            }

            return txt + ">";
        }

        public static GVariable CreateGVariable(string variableName)
        {
            return variableName?.Length <= 0 ? null : new GVariable(variableName);
        }

        public static string CastParameterString(GObject obj, Type type)
        {
            if (type == typeof(object))
                return obj?.ToSource();

            if (
                obj is GNumber && type == typeof(double) || obj is GString && type == typeof(string) ||
                obj is GLogic && type == typeof(bool) || obj is GList && type == typeof(List<object>) ||
                obj is ICustomObject customObject && type == customObject.CustomType
            )
            {
                return obj.ToSource();
            }

            var castString = GetTypeName(type);

            if (IsNumberType(type))
            {
                if (obj is GNumber)
                {
                    return $"({castString}){obj.ToSource()}";
                }

                return type == typeof(double)
                    ? $"{obj?.ToSource()}.ToNumber()"
                    : $"({castString}){obj?.ToSource()}.ToNumber()";
            }

            if (type == typeof(string))
            {
                return $"{obj?.ToSource()}.ToText()";
            }

            if (type == typeof(bool))
            {
                return $"{obj?.ToSource()}.ToBool()";
            }

            if (!IsListType(type))
                return type == typeof(object) ? obj?.ToSource() : $"{obj?.ToSource()}.ToCustom<{castString}>()";

            var listType = type.GetGenericArguments()[0];

            if (obj is GList || obj is ICustomObject o && IsListType(o.CustomType))
            {
                return $"{obj.ToSource()}.Select(e => e.ToCustom<{listType}>()).ToList()";
            }

            return listType == typeof(object)
                ? $"{obj?.ToSource()}.ToList()"
                : $"{obj?.ToSource()}.ToList().Select(e => e.ToCustom<{castString}>()).ToList()";
        }

        private static bool IsListType(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>));
        }

        private static bool IsNumberType(Type type)
        {
            return NumberTypes.Contains(type);
        }
    }
}
