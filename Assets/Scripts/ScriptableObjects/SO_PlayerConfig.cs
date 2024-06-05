using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerConfig", menuName = "SO/Character/PlayerConfig", order = 51)]
public class SO_PlayerConfig : SO_CharacterConfig
{
    [field: SerializeField, Space] public float movementSpeed { get; private set; }
    [field: SerializeField, Space] public float speedRotate { get; private set; }
    [field: SerializeField, Space] public float gravity { get; private set; }
}
