using GSharp.Packager.Commons;
using System.Linq;

namespace GSharp.Packager.Extensions
{
    static class ValueExtension
    {
        public static TResult GetValue<TFrom, TResult>(this object value) where TFrom : IValueAttribute<TResult>
        {
            var field = value.GetType().GetField(value.ToString());
            var attrs = field.GetCustomAttributes(typeof(TFrom), false) as TFrom[];

            return attrs.FirstOrDefault().Value;
        }
    }
}
