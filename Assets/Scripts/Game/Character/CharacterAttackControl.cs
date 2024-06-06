using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackControl : MonoBehaviour, IControl, IAttack
{
    public IController iController { get; set; }

    private CharacterMainController characterMainController;

    protected IList<IHealth> currentHealthTargets = new List<IHealth>(50);

    private int currentDamage;

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
    }

    protected virtual void OnEnable()
    {
        characterMainController.onSwitchState += OnSwitchController;
        characterMainController.GetControl<CharacterFOVControl>().onFOVEnter += OnFOVEnter;
        characterMainController.GetControl<CharacterFOVControl>().onFOVExit += OnFOVExit;
    }
    protected virtual void OnDisable()
    {
        characterMainController.onSwitchState -= OnSwitchController;
        characterMainController.GetControl<CharacterFOVControl>().onFOVEnter -= OnFOVEnter;
        characterMainController.GetControl<CharacterFOVControl>().onFOVExit -= OnFOVExit;
    }

    private void OnSwitchController(CharacterStateType state)
    {
        isEnabled = state.HasFlag(CharacterStateType.move) ? true : false;
    }
    protected virtual void OnFOVEnter(IController controller)
    {
        currentHealthTargets.Add(controller.GetControl<IHealth>());
    }
    protected virtual void OnFOVExit(IController controller)
    {
        currentHealthTargets.Remove(controller.GetControl<IHealth>());
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
            countCooldown = 0;
        }
    }
    public virtual void ApplyDamage(IHealth health)
    {
        health.TakeDamage(currentDamage);
        if (health.GetHealth() <= 0) currentHealthTargets.Remove(health);
        print("health: " + currentHealthTargets.Count);
    }
}
