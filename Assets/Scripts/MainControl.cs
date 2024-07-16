using UnityEngine;
using System;

public abstract class MainControl : MonoBehaviour, IGameObjectControl
{
    public IGameObjectController iController { get; set; }

    public virtual void Initialize()
    {
        SetController();
    }
    protected abstract void SetController();
    protected virtual void OnEnable()
    {
        SetController();
    }
    protected virtual void OnDisable()
    {

    }

}