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

        public static IVariable CreateIVariable(string variableName, Type variableType)
        {
            if (variableName?.Length <= 0) return null;

            // string인 경우
            if (variableType == typeof(string))
            {
                return new GStringVariable(variableName);
            }

            // bool인 경우
            if (variableType == typeof(bool))
            {
                return new GLogicVariable(variableName);
            }

            // 자료형이 숫자 형태인 경우
            if (numberTypes.Contains(variableType))
            {
                return new GNumberVariable(variableName);
            }

            // 이 외의 경우 모두 CustomVariable
            return new GCustomVariable(variableType, variableName);
        }
    }
}
