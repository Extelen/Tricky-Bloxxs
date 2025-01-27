using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    /// <summary>
    /// Get a random value from a list.
    /// </summary>
    public static T Random<T>(this List<T> list)
    {
        if (list == null)
        {
            Debug.LogError("List doesn't exists");
            return default;
        }

        if (list.Count == 0)
        {
            Debug.LogError("List count is equals to 0");
            return default;
        }

        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}
