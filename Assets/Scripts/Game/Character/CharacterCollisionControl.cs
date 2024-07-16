using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterCollisionControl : MainControl, ICollision
{
    [SerializeField] private CharacterMainController characterMainController;
    protected override void SetController()
    {
        iController = characterMainController;
    }
}
