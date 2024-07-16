using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_LevelConfig", menuName = "SO/Level", order = 51)]
public class SO_LevelConfig : ScriptableObject, IConfigSO
{
    public GameController gameController;
    public ZoneConfig[] zoneConfigs;
}

[System.Serializable]
public class ZoneConfig : IConfigSO
{
    public EnemyConfig[] enemyConfigs;

    [System.Serializable]
    public class EnemyConfig : IConfigSO
    {
        public SO_EnemyConfig so_EnemyConfig;
        public float cooldown;
        public int total;
    }

    public ZoneConfig(EnemyConfig[] enemyConfigs)
    {
        this.enemyConfigs = new EnemyConfig[enemyConfigs.Length];
        for (int i = 0; i < enemyConfigs.Length; i++)
        {
            this.enemyConfigs[i] = new EnemyConfig
            {
                so_EnemyConfig = enemyConfigs[i].so_EnemyConfig,
                cooldown = enemyConfigs[i].cooldown,
                total = enemyConfigs[i].total,
            };
        }
    }
}