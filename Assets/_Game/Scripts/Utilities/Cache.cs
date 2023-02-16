using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache<T> where T : MonoBehaviour
{
    private static Dictionary<Collider, T> dict = new Dictionary<Collider, T>();

    public static T Get(Collider collider)
    {
        if (!dict.ContainsKey(collider))
        {
            dict.Add(collider, collider.GetComponent<T>());
        }

        return dict[collider];
    }
}
