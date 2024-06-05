using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    var singleton = new GameObject($"[ {typeof(T)} ] ");
                    instance = singleton.AddComponent<T>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return instance;
        }
    }
}

public class SingletonSO<T> : ScriptableObject where T : ScriptableObject
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<T>($"ScriptableObjects/{typeof(T).Name}");
            }
            return instance;
        }
    }
}
