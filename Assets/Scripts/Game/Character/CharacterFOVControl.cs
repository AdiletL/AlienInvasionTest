using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFOVControl : MonoBehaviour, IControl
{
    public event Action<List<IController>> onFOVEnter;
    public IController iController { get; set; }

    private CharacterMainController characterMainController;

    private List<IController> foundControllers = new List<IController>(100);

    private float radius;
    private float angle;
    private float cooldown;
    private float countCooldown;

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

        radius = characterMainController.so_CharacterConfig.radiusFOV;
        angle = characterMainController.so_CharacterConfig.angleFOV;
        cooldown = characterMainController.so_CharacterConfig.cooldownFOV;
    }

    private void LateUpdate()
    {
        countCooldown += Time.deltaTime;

        if (countCooldown > cooldown)
        {
            FindObject();
            countCooldown = 0;
        }
    }

    private void FindObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, Layers.Enemy);
        foundControllers.Clear();
        for (int i = hitColliders.Length - 1; i >= 0; i--)
        {
            if (hitColliders[i].GetComponent<ICollision>() == null) continue;

            var target = GetTriggerTarget(hitColliders[i].transform.position);
            foundControllers.Add(target);
        }
        onFOVEnter?.Invoke(foundControllers);
    }
}
