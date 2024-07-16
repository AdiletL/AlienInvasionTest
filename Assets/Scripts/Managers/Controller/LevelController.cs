using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour, IController
{
    [field: SerializeField] public SO_LevelContainer so_LevelContainer { get; private set; }

    private GameController currentGameController;

    public void Initialize()
    {

    }
    public void SpawnLevel(int levelNumber)
    {
        currentGameController = Instantiate(so_LevelContainer.levels[levelNumber].gameController);
        currentGameController.transform.position = Vector3.zero;
        currentGameController.Initialize();
    }
}
