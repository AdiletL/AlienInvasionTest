using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMainController : MainBehaviour
{
    public event Action<CharacterStateType> onSwitchState;

    #region Serialize
    [field: SerializeField] public SO_CharacterConfig so_CharacterConfig { get; private set; }
    #endregion

    private List<CharacterActionState> actionStates;
    private CharacterStateType currentState;
    private CharacterStateType currentEnableState;

    protected override void Awake()
    {
        base.Awake();
        actionStates = new List<CharacterActionState>(so_CharacterConfig.actionStates.Length);
        foreach (var item in so_CharacterConfig.actionStates)
        {
            actionStates.Add(new CharacterActionState(item));
        }
    }

    private void OnEnable()
    {
        GetControl<CharacterHealthCotrol>().onRevival += OnRevival;
        GetControl<CharacterHealthCotrol>().onDeath += OnDeath;
    }
    private void OnDisable()
    {
        GetControl<CharacterHealthCotrol>().onRevival -= OnRevival;
        GetControl<CharacterHealthCotrol>().onDeath -= OnDeath;
    }

    private void OnRevival() => SwitchState(CharacterStateType.idle);
    private void OnDeath() => Die();

    public void SwitchState(CharacterStateType state)
    {
        if (state == currentState) return;

        currentState = state;
        currentEnableState = actionStates.Find(item => item.stateType == currentState).enableStateType;
        onSwitchState?.Invoke(currentEnableState);
    }

    public virtual void ReturnToPool()
    {
        PoolManager.Instance.ReturnObjectToPool(gameObject);
    }

    public override void Die()
    {
        base.Die();
        SwitchState(CharacterStateType.dead);
    }
}
