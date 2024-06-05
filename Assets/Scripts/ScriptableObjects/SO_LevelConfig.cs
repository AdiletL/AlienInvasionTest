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
        public EnemyType enemyType;
        public float cooldown;
        public int total;
    }
}