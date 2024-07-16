using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameZone : MonoBehaviour
{
    [SerializeField] private float radius;

    private ZoneConfig zoneConfig;

    private Coroutine enemyCoroutine;

    private Dictionary<Type, int> keyEnemyTotal;

    [HideInInspector] public int zoneID = -1;

    private Vector3 GetNewPosition()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Random.Range(0f, radius);
        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;
        return new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
    }

    public void Initialize(IConfigSO config)
    {
        var zoneConfig = config as ZoneConfig;
        if (zoneConfig == null)
        {
            enabled = false;
            return;
        }
        this.zoneConfig = new ZoneConfig(zoneConfig.enemyConfigs);

        keyEnemyTotal = new Dictionary<Type, int>(zoneConfig.enemyConfigs.Length);
        foreach (var item in zoneConfig.enemyConfigs)
            keyEnemyTotal.Add(EnemyFactory.Convert(item.so_EnemyConfig).GetType(), 0);
    }

    private void OnEnable()
    {
        EnemyController.OnReturnToPool += OnReturnToPoolEnemy;
    }
    private void OnDisable()
    {
        EnemyController.OnReturnToPool -= OnReturnToPoolEnemy;
    }

    private void OnReturnToPoolEnemy(Type data, int zoneID)
    {
        if (this.zoneID != zoneID || !keyEnemyTotal.ContainsKey(data)) return;
        else keyEnemyTotal[data]--;
    }

    public void StartSpawnEnemies()
    {
        if (enemyCoroutine != null) StopCoroutine(enemyCoroutine);
        else enemyCoroutine = StartCoroutine(SpawnEnemiesCoroutines());
    }

    private IEnumerator SpawnEnemiesCoroutines()
    {
        if (zoneConfig == null) yield break;

        var countTimes = new List<float>(zoneConfig.enemyConfigs.Length);
        foreach(var item in zoneConfig.enemyConfigs)
            countTimes.Add(item.cooldown);

        while (true)
        {
            if (!GameManager.IsPaused)
            {
                for (int i = 0; i < countTimes.Count; i++)
                {
                    countTimes[i] += Time.deltaTime;

                    if (countTimes[i] >= zoneConfig.enemyConfigs[i].cooldown)
                    {
                        var key = EnemyFactory.Convert(zoneConfig.enemyConfigs[i].so_EnemyConfig).GetType();
                        if (keyEnemyTotal[key] < zoneConfig.enemyConfigs[i].total)
                        {
                            var enemyGO = EnemyFactory.Create(zoneConfig.enemyConfigs[i].so_EnemyConfig);
                            enemyGO.transform.position = GetNewPosition();
                            enemyGO.transform.SetParent(transform);
                            var enemy = enemyGO.GetComponent<EnemyController>();
                            enemy.currentZoneID = zoneID;
                            enemy.GetControl<EnemyHealthControl>().Revival();
                            keyEnemyTotal[key]++;
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
