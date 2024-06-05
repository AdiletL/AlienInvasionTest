using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameZone : MonoBehaviour
{
    [SerializeField] private float radius;

    private ZoneConfig zoneConfig;

    private Coroutine enemyCoroutine;

    private Dictionary<EnemyType, int> keyEnemyTotal;

    private Vector3 GetNewPosition()
    {
        return Random.insideUnitCircle * radius;
    }

    public void Initialize(IConfigSO config)
    {
        zoneConfig = config as ZoneConfig;
        if (zoneConfig == null)
        {
            enabled = false;
            return;
        }

        keyEnemyTotal = new Dictionary<EnemyType, int>(zoneConfig.enemyConfigs.Length);
        foreach (var item in zoneConfig.enemyConfigs)
            keyEnemyTotal.Add(item.enemyType, item.total);

    }

    public void StartSpawnEnemies()
    {
        if (enemyCoroutine != null) StopCoroutine(enemyCoroutine);
        else enemyCoroutine = StartCoroutine(SpawnEnemiesCoroutines());
    }

    private IEnumerator SpawnEnemiesCoroutines()
    {
        var countTimes = new List<float>(zoneConfig.enemyConfigs.Length);
        foreach(var item in zoneConfig.enemyConfigs)
            countTimes.Add(item.cooldown);

        while (true)
        {
            if (!GameMainManager.IsPaused)
            {
                for (int i = 0; i < countTimes.Count; i++)
                {
                    countTimes[i] += Time.deltaTime;

                    if (countTimes[i] >= zoneConfig.enemyConfigs[i].cooldown)
                    {
                        if (keyEnemyTotal[zoneConfig.enemyConfigs[i].enemyType] < zoneConfig.enemyConfigs[i].total)
                        {
                            var enemy = PoolManager.Instance.GetObject<EnemyMainController>();
                            enemy.transform.position = GetNewPosition();
                            enemy.transform.SetParent(transform);
                        }

                        countTimes[i] = 0;
                    }
                }
            }
            yield return null;
        }
    }

    public void StopSpawnEnemies()
    {
        if (enemyCoroutine != null) StopCoroutine(enemyCoroutine);
    }
}
