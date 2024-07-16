using System.Collections;
using UnityEngine;


public abstract class GameObjectController : MainController
{
    protected abstract void Awake();
    public abstract void Revival();
    public abstract void Die();
}
