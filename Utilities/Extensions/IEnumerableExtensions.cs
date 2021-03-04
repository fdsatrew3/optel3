using System;
using System.Collections.Generic;

namespace Utilities.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection), "Collection is null");

            foreach(var item in collection)
            {
                action(item);
            }
        }
    }
}
