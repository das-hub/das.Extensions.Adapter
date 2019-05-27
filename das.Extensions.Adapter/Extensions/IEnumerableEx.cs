using System;
using System.Collections.Generic;

namespace das.Extensions.Adapter.Extensions
{
    internal static class IEnumerable
    {
        internal static void Do<T>(this IEnumerable<T> collection, Action<T> action) where T : class
        {
            if (collection == null)
                return;

            foreach (T item in collection) action(item);
        }

        internal static void Do<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int index = -1;
            foreach (T element in source)
            {
                index++;
                action(element, index);
            }
        }
    }
}
