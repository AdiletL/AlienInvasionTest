using System;
using System.Collections.Generic;
using UnityEngine;

public class GameMainManager : MonoBehaviour, IManager
{
    public static GameMainManager Instance { get; private set; }

    public static event Action<GameStateType> OnSetGameStateType;
    public static bool IsPaused { get; private set; }

    private List<GameObject> objectManagers = new List<GameObject>(10);


    public T GetManager<T>()
    {
        foreach (var item in objectManagers)
        {
            var manager = item.GetComponent<T>();
            if (manager == null) continue;
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

        InitializeLink();
        Initialize();
    }
    private void InitializeLink()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            objectManagers.Add(transform.GetChild(i).gameObject);
        }
    }
    public void Initialize()
    {
        foreach (var item in objectManagers)
        {
            var manager = item.GetComponent<IManager>();
            if (manager == null) continue;
            manager.Initialize();
        }
    }

    private void Start()
    {
        GetManager<LevelManager>().StartLevel();
    }
}
