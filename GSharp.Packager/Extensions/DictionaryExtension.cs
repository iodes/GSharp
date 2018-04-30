using System.Collections.Generic;

namespace GSharp.Packager.Extensions
{
    static class DictionaryExtension
    {
        public static TValue Find<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return default(TValue);
        }
    }
}
