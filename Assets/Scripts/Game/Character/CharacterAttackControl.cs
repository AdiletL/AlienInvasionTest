using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAttackControl : MonoBehaviour, IControl, IAttack
{
    public event Action<bool> OnAttack;

    public IController iController { get; set; }

    private CharacterMainController characterMainController;

    protected IList<IHealth> currentHealthTargets = new List<IHealth>(50);

    private int currentDamage;
    private int totalTarget;

    private float cooldown;
    private float countCooldown;

    private bool isEnabled;


    public virtual void Initialize(IController controller)
    {
        iController = controller;
        characterMainController = iController as CharacterMainController;
        if (characterMainController == null)
        {
            enabled = false;
            return;
        }

        currentDamage = characterMainController.so_CharacterConfig.damage;
        cooldown = characterMainController.so_CharacterConfig.attackCooldown;
        totalTarget = characterMainController.so_CharacterConfig.totalTarget;
    }

    protected virtual void OnEnable()
    {
        characterMainController.onSwitchState += OnSwitchController;
        characterMainController.GetControl<CharacterFOVControl>().onFOVEnter += OnFOVEnter;
    }
    protected virtual void OnDisable()
    {
        characterMainController.onSwitchState -= OnSwitchController;
        characterMainController.GetControl<CharacterFOVControl>().onFOVEnter -= OnFOVEnter;
    }

    private void OnSwitchController(CharacterStateType state)
    {
        isEnabled = state.HasFlag(CharacterStateType.move) ? true : false;
    }
    protected virtual void OnFOVEnter(List<IController> controller)
    {
        var healthes = new List<IHealth>();
        for (int i = controller.Count - 1; i >= 0; i--)
        {
            if (controller[i] == null) continue;
            healthes.Add(controller[i].GetControl<IHealth>());
        }

        currentHealthTargets = currentHealthTargets.Intersect(healthes).ToList();
        currentHealthTargets = currentHealthTargets.Take(totalTarget).ToList();
        int countController = currentHealthTargets.Count;
        if (countController >= totalTarget) return;

        for (int i = controller.Count - 1; i >= 0; i--)
        {
            if (countController > totalTarget) return;
            else countController++;

            if (controller[i] == null) continue;
            currentHealthTargets.Add(controller[i].GetControl<IHealth>());
        }

        if (currentHealthTargets.Count == 0) OnAttack?.Invoke(false);
    }


    private void Update()
    {
        if (!isEnabled)
        {
            countCooldown = 0;
            return;
        }

        Attack();
    }
    public virtual void Attack()
    {
        if (currentHealthTargets.Count == 0) return;

        countCooldown += Time.deltaTime;

        if (cooldown <= countCooldown)
        {
            var targets = new List<IHealth>(currentHealthTargets.Count);
            for (int i = 0; i < currentHealthTargets.Count; i++)
            {
                targets.Add(currentHealthTargets[i]);
            }

            foreach (var item in targets)
            {
                if(item == null || item.GetHealth() <= 0) continue;
                ApplyDamage(item);
            }
            OnAttack?.Invoke(true);
            countCooldown = 0;
        }
    }
    public virtual void ApplyDamage(IHealth health)
    {
        health.TakeDamage(currentDamage);
        if (health.GetHealth() <= 0) currentHealthTargets.Remove(health);
    }
}
