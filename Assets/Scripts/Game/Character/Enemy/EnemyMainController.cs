using System;
using System.Collections;
using UnityEngine;


public abstract class EnemyMainController : CharacterMainController
{
    public static event Action<EnemyType, int> OnReturnToPool;

    [field: SerializeField] public EnemyType enemyType { get; protected set; }

    [HideInInspector] public int currentZoneID = -1;

    protected virtual void Start()
    {
        SwitchState(CharacterStateType.idle);
    }

    public override void ReturnToPool()
    {
        base.ReturnToPool();
        OnReturnToPool?.Invoke(enemyType, currentZoneID);
    }
}
