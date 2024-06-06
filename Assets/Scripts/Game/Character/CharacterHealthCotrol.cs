using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthCotrol : MonoBehaviour, IControl, IHealth
{
    public event Action<int, int> onSetHealth;
    public event Action onRevival;
    public event Action onDeath;

    public IController iController { get; set; }

    private CharacterMainController characterMainController;

    private int maxHealth;
    private int currentHealth;

    private bool isEnabled = true;

    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public int GetHealth()
    {
        return currentHealth;
    }
    private void SetHealh(int value)
    {
        if (!isEnabled || value == currentHealth) return;

        currentHealth = value;
        onSetHealth?.Invoke(maxHealth, currentHealth);

        if (currentHealth <= 0)
        {
            onDeath?.Invoke();
        }
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

        maxHealth = characterMainController.so_CharacterConfig.maxHealth;
        //SetHealh(maxHealth);
    }

    protected virtual void OnEnable()
    {
        characterMainController.onSwitchState += OnSwitchController;
    }
    protected virtual void OnDisable()
    {
        characterMainController.onSwitchState -= OnSwitchController;
    }

    protected virtual void Start()
    {
        SetHealh(maxHealth);
    }

    private void OnSwitchController(CharacterStateType state)
    {
        isEnabled = state.HasFlag(CharacterStateType.move) ? true : false;
    }
    public void Revival()
    {
        onRevival?.Invoke();
        SetHealh(maxHealth);
    }
    public void TakeDamage(int damage)
    {
        var result = Mathf.Max(currentHealth - damage, 0);
        SetHealh(result);
    }
}
