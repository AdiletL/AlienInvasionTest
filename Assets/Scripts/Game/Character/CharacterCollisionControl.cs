using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionControl : MonoBehaviour, IControl, ICollision
{
    public IController iController { get; set; }

    public virtual void Initialize(IController controller)
    {
        iController = controller;
    }
}
