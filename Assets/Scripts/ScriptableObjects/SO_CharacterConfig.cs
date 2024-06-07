using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_CharacterConfig : ScriptableObject, IConfigSO
{
    [field: SerializeField, Header("Setting up actions")] public CharacterActionState[] actionStates { get; private set; }
    [field: SerializeField, Space(10)] public int maxHealth { get; private set; }
    [field: SerializeField, Space] public int damage { get; private set; }
    [field: SerializeField, Space] public int totalTarget { get; private set; }
    [field: SerializeField, Space] public float attackCooldown { get; private set; } = 1;
    [field: SerializeField, Space] public float radiusFOV { get; private set; }
    [field: SerializeField, Space] public float angleFOV { get; private set; } = 360;
    [field: SerializeField, Space] public float cooldownFOV { get; private set; } = .5f;
}
[System.Serializable]
public class CharacterActionState
{
    public CharacterStateType stateType;
    public CharacterStateType enableStateType;

    public CharacterActionState(CharacterActionState newActionState)
    {
        stateType = newActionState.stateType;
        enableStateType = newActionState.enableStateType;
    }
}
