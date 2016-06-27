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
            var compiler = new CSharpCodeProvider();
            var castType = new CodeTypeReference(type);
            string castString = compiler.GetTypeOutput(castType);
            
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
                    return string.Format("({0}){1}", castString, obj?.ToSource());
                }

                if (type == typeof(double))
                {
                    return string.Format("{0}.ToNumber()", obj?.ToSource());
                }

                return string.Format("({0}){1}.ToNumber()", castString, obj?.ToSource());
            }

            if (type == typeof(string))
            {
                return string.Format("{0}.ToText()", obj?.ToSource());
            }

            if (type == typeof(bool))
            {
                return string.Format("{0}.ToBool()", obj?.ToSource());
            }

            // is List
            if (IsListType(type))
            {
                Type listType = type.GetGenericArguments()[0];

                if (obj is GList || obj is ICustom && IsListType((obj as ICustom).CustomType))
                {
                    return string.Format("{0}.Select(e => e.ToCustom<{1}>()).ToList()", obj?.ToSource(), listType.ToString());
                }
                
                if (listType == typeof(object))
                {
                    return string.Format("{0}.ToList()", obj?.ToSource());
                }

                var listCastType = new CodeTypeReference(listType);
                string listCastString = compiler.GetTypeOutput(castType);

                return string.Format("{0}.ToList().Select(e => e.ToCustom<{1}>()).ToList()", obj?.ToSource(), listCastString);
            }

            if (type == typeof(object))
            {
                return obj?.ToSource();
            }

            return string.Format("{0}.ToCustom<{1}>()", obj?.ToSource(), castString);
        }

        public static bool IsListType(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>));
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
