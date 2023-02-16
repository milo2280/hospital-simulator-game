using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<T>();

            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
            }

            return instance;
        }
    }
}

public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

#if PHOTON_UNITY_NETWORKING
public abstract class PUNSingleton<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<T>();

            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
            }

            return instance;
        }
    }
}

public abstract class PersistentPUNSingleton<T> : PUNSingleton<T> where T : MonoBehaviourPunCallbacks
{
    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
#endif