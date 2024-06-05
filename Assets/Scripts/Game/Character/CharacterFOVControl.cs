using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFOVControl : MonoBehaviour, IControl
{
    public event Action<IController> onFOVEnter;
    public event Action<IController> onFOVExit;

    public IController iController { get; set; }

    public void Initialize(IController controller)
    {
        iController = controller;

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IController controller))
        {
            onFOVEnter?.Invoke(controller);
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IController controller))
        {
            onFOVExit?.Invoke(controller);
        }
    }
}
