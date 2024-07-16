using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IManager
{
    public static GameManager Instance { get; private set; }

    public static event Action<GameStateType> OnSetGameStateType;
    public static bool IsPaused { get; private set; }

    private List<IManager> managers;

    public T GetManager<T>()
    {
        foreach (var item in managers)
        {
            if (item is T manager)
                return manager;
        }
        return default;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        Application.targetFrameRate = 120;
#endif

        Initialize();
    }

    public void Initialize()
    {
        managers = new List<IManager>(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            var currentManagers = transform.GetChild(i).GetComponents<IManager>();
            foreach (var item in currentManagers)
            {
                managers.Add(item);
                item.Initialize();
            }
        }
    }

    private void Start()
    {
        GetManager<LevelManager>().StartLevel();
    }
}
