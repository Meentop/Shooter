using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionsExtensions
{
    public static int GetNextIndex<T>(this ICollection<T> collection, int currentIdex)
    {
        var nextIndex = currentIdex + 1;

        return nextIndex >= collection.Count ? 0 : nextIndex < 0 ? collection.Count - 1 : nextIndex;
    }
}
