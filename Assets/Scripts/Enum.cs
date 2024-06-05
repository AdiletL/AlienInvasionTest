
using System;

public enum GameStateType
{
    nothing,
    loading,
    resume,
    pause,
    exit,
}

[Flags]
public enum CharacterStateType
{
    nothing,
    idle = 1 << 0,
    move = 1 << 1,
    attack = 1 << 2,
    health = 1 << 3,
    disable = 1 << 4,
    dead = 1 << 5,
}

public enum EnemyType
{
    nothing,
    tiny,
    small,
    medium,
    large,
    huge
}

[Flags]
public enum GameStageType
{
    nothing,
    start = 1 << 0,
    end = 1 << 2,
}