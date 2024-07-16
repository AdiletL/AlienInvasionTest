using UnityEngine;

public static class EnemyFactory
{
    public static EnemyData Convert(SO_EnemyConfig config)
    {
        EnemyData enemy = null;
        switch (config)
        {
            case SO_OgreConfig:
                enemy = new OgreData();
                break;
            case SO_CreepConfig:
                enemy = new CreepData();
                break;
            default:
                break;
        }
        return enemy;
    }
    public static GameObject Create(SO_EnemyConfig config)
    {
        GameObject gameObject = null;
        switch (config)
        {
            case SO_OgreConfig:
                gameObject = PoolManager.Instance.GetObject<OgreController>();
                break;
            case SO_CreepConfig:
                gameObject = PoolManager.Instance.GetObject<CreepController>();
                break;
            default:
                break;
        }
        return gameObject;
    }
}