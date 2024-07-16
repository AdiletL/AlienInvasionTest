using System;
using System.Collections;
using UnityEngine;


public abstract class EnemyController : CharacterMainController
{
    public static event Action<Type, int> OnReturnToPool;

    [HideInInspector] public int currentZoneID = -1;

    protected EnemyData data;

    protected abstract EnemyData CreateData();

    public override void Initialize()
    {
        base.Initialize();
        data = CreateData();
    }

    protected virtual void Start()
    {
        SwitchState(CharacterStateType.idle);
    }

    public override void ReturnToPool()
    {
        base.ReturnToPool();
        OnReturnToPool?.Invoke(data.GetType(), currentZoneID);
    }
}

public class EnemyData : IData
{

}