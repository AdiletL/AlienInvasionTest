using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour, IController
{
    public static event Action OnStarted;
    public static event Action OnFinished;

    public GameStageType currentGameStageType { get; private set; }

    [SerializeField] private GameZone[] zones;
    [SerializeField] private Transform playerSpawnPosition;

    private PlayerController playerController;

    public void Initialize()
    {
        var levelController = FindObjectOfType<LevelController>();
        if (levelController == null)
        {
            enabled = false;
            return;
        }

        InitializeZones(levelController.so_LevelContainer.levels[0]);
        SwitchStage(GameStageType.start);
    }
    private void InitializeZones(SO_LevelConfig levelConfig)
    {
        for (int i = 0; i < zones.Length; i++)
        {
            if (levelConfig.zoneConfigs.Length - 1 < i) 
            {
                zones[i].zoneID = -1;
            }
            else
            {
                zones[i].Initialize(levelConfig.zoneConfigs[i]);
                zones[i].zoneID = i;
            }
        }
    }
    public T GetControl<T>()
    {
        throw new System.NotImplementedException();
    }

    private void SwitchStage(GameStageType stageType)
    {
        if (stageType == currentGameStageType) return;

        RemoveCurrentGameStageType(currentGameStageType);
        AddCurrentGameStageType(stageType);

        switch (stageType)
        {
            case GameStageType.start:
                StartGame();
                break;
            case GameStageType.end:
                EndGame();
                break;
        }
    }
    private void AddCurrentGameStageType(GameStageType targetType) => currentGameStageType |= targetType;
    private void RemoveCurrentGameStageType(GameStageType targetType) => currentGameStageType &= ~targetType;


    private void StartGame()
    {
        SpawnPlayer();
        SpawnEnemies();
    }

    private void EndGame()
    {

        OnFinished?.Invoke();
    }


    private void SpawnPlayer()
    {
        playerController = PoolManager.Instance.GetObject<PlayerController>().GetComponent<PlayerController>();
        playerController.transform.position = playerSpawnPosition.position;
    }

    private void SpawnEnemies()
    {
        foreach (var item in zones)
            item.StartSpawnEnemies();
    }
}