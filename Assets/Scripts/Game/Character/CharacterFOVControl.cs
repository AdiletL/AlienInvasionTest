using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFOVControl : MonoBehaviour, IControl
{
    public event Action<IController> onFOVEnter;
    public event Action<IController> onFOVExit;
    public IController iController { get; set; }

    private CharacterMainController characterMainController;
    private SphereCollider sphereCollider;

    private float radius;
    private float angle;

    protected IController GetTriggerTarget(Vector3 endPoint)
    {
        Vector3 direction = (endPoint - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, radius))
        {
            if (hit.transform.TryGetComponent(out IControl control))
            {
                return control.iController;
            }
        }
        return null;
    }

    public virtual void Initialize(IController controller)
    {
        iController = controller;
        characterMainController = iController as CharacterMainController;
        if (characterMainController == null)
        {
            enabled = false;
            return;
        }

        sphereCollider = GetComponent<SphereCollider>();

        radius = characterMainController.so_CharacterConfig.radiusFOV;
        angle = characterMainController.so_CharacterConfig.angleFOV;
        sphereCollider.radius = radius;
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ICollision collision))
        {
            var target = GetTriggerTarget(other.transform.position);
            if (target != null) onFOVEnter?.Invoke(target);
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
