using UnityEngine;

public static class Layers
{
    #region LayerMask
    public readonly static LayerMask Player = 1 << 3, Enemy = 1 << 6;
    #endregion

    #region LayerIndex
    public readonly static int PlayerIndex = 3, EnemyIndex = 6;
    #endregion
}
