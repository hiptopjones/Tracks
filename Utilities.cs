using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal static class Utilities
    {
        public static void DeleteWithSwapAndPop<T>(List<T> collection, Predicate<T> deletePredicate)
        {
            // Avoiding "foreach" loop as we may mutate the list
            // Avoiding "for" loop as we're using swap and pop
            int i = 0;
            while (i < collection.Count)
            {
                if (deletePredicate.Invoke(collection[i]))
                {
                    // Swap and pop to avoid shifting elements in an array list
                    int lastIndex = collection.Count - 1;
                    collection[i] = collection[lastIndex];
                    collection.RemoveAt(lastIndex);
                }
                else
                {
                    i++;
                }
            }
        }

        // Creates a key that uniquely represents a pair of objects, regardless of the order they are provided
        // TODO: Consider Szudsik's elegant pairing algorithm for this (more efficient)
        public static string MakeKey(int id1, int id2)
        {
            if (id1 < id2)
            {
                return $"{id1}-{id2}";
            }
            else
            {
                return $"{id2}-{id1}";
            }
        }
    }
}
