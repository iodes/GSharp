using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Base.Objects.Customs;
using GSharp.Base.Objects.Logics;
using GSharp.Base.Objects.Numbers;
using GSharp.Base.Objects.Strings;
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Utilities
{
    public static class GSharpUtils
    {
        private static readonly Type[] numberTypes = {
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double)
        };

        public static GVariable CreateGVariable(string variableName)
        {
            return variableName?.Length <= 0 ? null : new GVariable(variableName);
        }

        public static string CastParameterString(GObject obj, Type type)
        {
            var compiler = new CSharpCodeProvider();
            var castType = new CodeTypeReference(type);
            var castString = compiler.GetTypeOutput(castType);
            
            // 변환이 필요없음
            if (type == typeof(object))
            {
                return obj?.ToSource();
            }

            // 같은 타입임
            if (
                obj is GNumber && type == typeof(double) ||
                obj is GString && type == typeof(string) ||
                obj is GLogic && type == typeof(bool) ||
                obj is GList && type == typeof(List<object>) ||
                obj is ICustom && type == (obj as ICustom).CustomType
            )
            {
                return obj?.ToSource();
            }

            // To Number
            if (IsNumberType(type))
            {
                if (obj is GNumber)
                {
                    return $"({castString}){obj?.ToSource()}";
                }

                return type == typeof(double) ? $"{obj?.ToSource()}.ToNumber()" : $"({castString}){obj?.ToSource()}.ToNumber()";
            }

            if (type == typeof(string))
            {
                return $"{obj?.ToSource()}.ToText()";
            }

            if (type == typeof(bool))
            {
                return $"{obj?.ToSource()}.ToBool()";
            }

            // is List
            if (IsListType(type))
            {
                Type listType = type.GetGenericArguments()[0];

                if (obj is GList || obj is ICustom && IsListType((obj as ICustom).CustomType))
                {
                    return $"{obj?.ToSource()}.Select(e => e.ToCustom<{listType.ToString()}>()).ToList()";
                }
                
                if (listType == typeof(object))
                {
                    return $"{obj?.ToSource()}.ToList()";
                }

                var listCastType = new CodeTypeReference(listType);
                string listCastString = compiler.GetTypeOutput(castType);

                return $"{obj?.ToSource()}.ToList().Select(e => e.ToCustom<{listCastString}>()).ToList()";
            }

            if (type == typeof(object))
            {
                return obj?.ToSource();
            }

            return $"{obj?.ToSource()}.ToCustom<{castString}>()";
        }

        public static bool IsListType(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>));
        }

        public static bool IsNumberType(Type type)
        {
            return numberTypes.Contains(type);
        }
    }
}
