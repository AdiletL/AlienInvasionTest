using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameZone : MonoBehaviour
{
    [SerializeField] private float radius;

    private ZoneConfig zoneConfig;

    private Coroutine enemyCoroutine;

    private Dictionary<EnemyType, int> keyEnemyTotal;

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
        this.zoneConfig = new ZoneConfig(zoneConfig);

        keyEnemyTotal = new Dictionary<EnemyType, int>(zoneConfig.enemyConfigs.Length);
        foreach (var item in zoneConfig.enemyConfigs)
            keyEnemyTotal.Add(item.enemyType, 0);
    }

    private void OnEnable()
    {
        EnemyMainController.OnReturnToPool += OnReturnToPoolEnemy;
    }
    private void OnDisable()
    {
        EnemyMainController.OnReturnToPool -= OnReturnToPoolEnemy;
    }

    private void OnReturnToPoolEnemy(EnemyType enemyType, int zoneID)
    {
        if (this.zoneID != zoneID || !keyEnemyTotal.ContainsKey(enemyType)) return;
        else keyEnemyTotal[enemyType]--;
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
            if (!GameMainManager.IsPaused)
            {
                for (int i = 0; i < countTimes.Count; i++)
                {
                    countTimes[i] += Time.deltaTime;

                    if (countTimes[i] >= zoneConfig.enemyConfigs[i].cooldown)
                    {
                        if (keyEnemyTotal[zoneConfig.enemyConfigs[i].enemyType] < zoneConfig.enemyConfigs[i].total)
                        {
                            var enemyGO = PoolManager.Instance.GetEnemy(zoneConfig.enemyConfigs[i].enemyType);
                            enemyGO.transform.position = GetNewPosition();
                            enemyGO.transform.SetParent(transform);
                            var enemy = enemyGO.GetComponent<EnemyMainController>();
                            enemy.currentZoneID = zoneID;
                            enemy.GetControl<EnemyHealthControl>().Revival();
                            keyEnemyTotal[enemy.enemyType]++;
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
