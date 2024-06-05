using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IManager
{
    private LevelController levelController;

    public void Initialize()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).GetComponent<MonoBehaviour>();
            if (child == null) break;

            if(levelController == null) levelController = child as LevelController;
        }
    }

    public void StartLevel()
    {
        levelController.SpawnLevel(0);
    }
    private void NextLevel()
    {

    }
    private void RestartLevel()
    {

    }
}
