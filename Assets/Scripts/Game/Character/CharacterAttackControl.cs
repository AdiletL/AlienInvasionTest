using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackControl : MonoBehaviour, IControl, IAttack
{
    public IController iController { get; set; }

    private CharacterMainController characterMainController;

    protected IList<IHealth> currentTarget = new List<IHealth>(50);

    private int currentDamage;

    private float cooldown;
    private float countCooldown;

    private bool isEnabled;


    public void Initialize(IController controller)
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

    private void Start()
    {
        InitializeEvent();
    }
    private void InitializeEvent()
    {
        characterMainController.onSwitchState += OnSwitchController;
        characterMainController.GetControl<CharacterFOVControl>().onFOVEnter += OnFOVEnter;
        characterMainController.GetControl<CharacterFOVControl>().onFOVExit += OnFOVExit;
    }
    private void DeInitializeEvent()
    {
        characterMainController.onSwitchState -= OnSwitchController;
        characterMainController.GetControl<CharacterFOVControl>().onFOVEnter -= OnFOVEnter;
        characterMainController.GetControl<CharacterFOVControl>().onFOVExit -= OnFOVExit;
    }
    private void OnSwitchController(CharacterStateType state)
    {
        isEnabled = state.HasFlag(CharacterStateType.move) ? true : false;
    }
    private void OnFOVEnter(IController controller)
    {
        currentTarget.Add(controller.GetControl<IHealth>());
    }
    private void OnFOVExit(IController controller)
    {
        currentTarget.Remove(controller.GetControl<IHealth>());
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
    public void Attack()
    {
        countCooldown += Time.deltaTime;

        if (cooldown < countCooldown)
        {
            var targets = new List<IHealth>(currentTarget.Count);
            for (int i = 0; i < currentTarget.Count; i++)
            {
                targets.Add(currentTarget[i]);
            }

            foreach (var item in targets)
            {
                if(item == null || item.GetHealth() <= 0) continue;
                ApplyDamage(item);
            }
            countCooldown = 0;
        }
    }
    public void ApplyDamage(IHealth health)
    {
        health.TakeDamage(currentDamage);
    }


    private void OnDestroy()
    {
        DeInitializeEvent();
    }
}
