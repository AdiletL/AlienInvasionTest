using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private GameObject gameMainManager;

    private async void Awake()
    {
        Instantiate(gameMainManager);

        // Загрузка следующей сцены асинхронно
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);

        // Ожидание завершения загрузки сцены
        while (!asyncLoad.isDone)
        {
            await System.Threading.Tasks.Task.Yield();
        }
    }
}
