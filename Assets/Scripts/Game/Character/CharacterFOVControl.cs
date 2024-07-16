using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterFOVControl : MainControl
{
    public event Action<List<IGameObjectController>> onFOVEnter;

    [SerializeField] private CharacterMainController characterMainController;

    private List<IGameObjectController> foundControllers = new List<IGameObjectController>(100);

    private float radius;
    private float cooldown;
    private float countCooldown;

    protected IGameObjectController GetCheckController(Vector3 endPoint)
    {
        Vector3 direction = (endPoint - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, radius))
        {
            if (hit.transform.TryGetComponent(out IGameObjectControl control))
            {
                return control.iController;
            }
        }
        return null;
    }

    public override void Initialize()
    {
        base.Initialize();
        radius = characterMainController.so_CharacterConfig.radiusFOV;
        cooldown = characterMainController.so_CharacterConfig.cooldownFOV;
    }
    protected override void SetController()
    {
        iController = characterMainController;
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

            var target = GetCheckController(hitColliders[i].transform.position);
            foundControllers.Add(target);
        }
        onFOVEnter?.Invoke(foundControllers);
    }
}
