using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_LevelContainer", menuName = "SO/LevelContainer", order = 51)]
public class SO_LevelContainer : ScriptableObject
{
    [field: SerializeField] public SO_LevelConfig[] levels { get; private set; }

}
