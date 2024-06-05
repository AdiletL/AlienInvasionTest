using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour, IController
{
    [field: SerializeField] public SO_LevelContainer so_LevelContainer { get; private set; }

    private GameController currentGameController;

    public T GetControl<T>()
    {
        throw new System.NotImplementedException();
    }

    public void Initialize(IController component)
    {
        
    }

    public void SpawnLevel(int levelNumber)
    {
        currentGameController = Instantiate(so_LevelContainer.levels[levelNumber].gameController);
        currentGameController.transform.position = Vector3.zero;
        currentGameController.Initialize(this);
    }
}
