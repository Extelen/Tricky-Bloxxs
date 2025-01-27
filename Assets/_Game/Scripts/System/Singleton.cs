using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Properties
    public static T Instance { get; private set; }
    protected virtual bool Persistent => false;

    // Methods
    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;

        if (Persistent)
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }
}