using System.Collections.Generic;

namespace GSharp.Packager.Extensions
{
    static class ListExtension
    {
        public static void Set<T>(this IList<T> list, IEnumerable<T> collection)
        {
            list.Clear();
            list.AddRange(collection);
        }

        public static bool AddSafe<T>(this IList<T> list, T item)
        {
            if (list.Contains(item))
                return false;

            list.Add(item);

            return true;
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                list.Add(item);
            }
        }

        public static bool RemoveSafe<T>(this IList<T> list, T item)
        {
            if (list.Contains(item))
                return false;

            list.Remove(item);

            return true;
        }

        public static bool RemoveAtSafe<T>(this IList<T> list, int index)
        {
            if (index > list.Count || index < 0)
                return false;

            list.RemoveAt(index);
            
            return true;
        }

        public static bool InsertSafe<T>(this IList<T> list, int index, T item)
        {
            if (index > list.Count || index < 0)
                return false;

            list.Insert(index, item);

            return true;
        }
    }
}
