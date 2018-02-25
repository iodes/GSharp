using System;

namespace GSharp.Bootstrap.Extensions
{
    internal static class NumericExtension
    {
        public static bool IsNumeric(this object target)
        {
            switch (Type.GetTypeCode(target.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;

                default:
                    if (double.TryParse(target.ToString(), out double result))
                    {
                        return true;
                    }

                    return false;
            }
        }
    }
}
