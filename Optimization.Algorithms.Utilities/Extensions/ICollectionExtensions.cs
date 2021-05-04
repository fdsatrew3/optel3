using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimization.Algorithms.Utilities.Extensions
{
    public static class ICollectionExtensions
    {
        public static int IndexOf<T>(this ICollection<T> collection, T item)
        {
            var index = -1;

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection.ElementAt(i).Equals(item))
                    return i;
            }

            return index;
        }

        public static int ListIndexOf<T>(this ICollection<T> collection, T item)
        {
            if (collection is List<T> list)
                return list.IndexOf(item);
            else
                return collection.IndexOf(item);
        }

        public static void Insert<T>(this ICollection<T> collection, int index, T item)
        {
            var oldPart = collection.Skip(index).ToArray();

            foreach(var it in oldPart)
            {
                collection.Remove(it);
            }

            collection.Add(item);

            foreach (var it in oldPart)
            {
                collection.Add(it);
            }
        }

        public static void ListInsert<T>(this ICollection<T> collection, int index, T item)
        {
            if (collection is List<T> list)
                list.Insert(index, item);
            else
                collection.Insert(index, item);
        }

        public static void RemoveAll<T>(this ICollection<T> collection, Predicate<T> check)
        {
            var array = collection.ToArray();

            foreach(var item in array)
            {
                if (check(item))
                    collection.Remove(item);
            }
        }

        public static void ListRemoveAll<T>(this ICollection<T> collection, Predicate<T> check)
        {
            if (collection is List<T> list)
                list.RemoveAll(check);
            else
                collection.RemoveAll(check);
        }

        public static void InsertRange<T>(this ICollection<T> collection, int position, IEnumerable<T> collectionToAdd)
        {
            var oldPart = collection.Skip(position).ToArray();

            oldPart.ForEach(x => collection.Remove(x));

            collectionToAdd.ForEach(x => collection.Add(x));

            oldPart.ForEach(x => collection.Add(x));
        }

        public static void ListInsertRange<T>(this ICollection<T> collection, int position, IEnumerable<T> collectionToAdd)
        {
            if (collection is List<T> list)
                list.InsertRange(position, collectionToAdd);
            else
                collection.InsertRange(position, collectionToAdd);
        }
    }
}
