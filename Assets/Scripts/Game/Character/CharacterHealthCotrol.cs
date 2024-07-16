using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterHealthCotrol : MainControl, IHealth
{
    public event Action<int, int> onSetHealth;
    public event Action onRevival;
    public event Action onDeath;

    [SerializeField] private CharacterMainController characterMainController;

    private int maxHealth;
    private int currentHealth;

    private bool isEnabled = true;

    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    private void SetHealth(int value)
    {
        if (!isEnabled || value == currentHealth) return;

        currentHealth = value;
        onSetHealth?.Invoke(maxHealth, currentHealth);

        if (currentHealth <= 0)
        {
            onDeath?.Invoke();
        }
    }

    public override void Initialize()
    {
        maxHealth = characterMainController.so_CharacterConfig.maxHealth;
        //SetHealh(maxHealth);
    }

    protected override void SetController()
    {
        iController = characterMainController;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        characterMainController.onSwitchState += OnSwitchController;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        characterMainController.onSwitchState -= OnSwitchController;
    }

    protected virtual void Start()
    {
        SetHealth(maxHealth);
    }

    private void OnSwitchController(CharacterStateType state)
    {
        isEnabled = state.HasFlag(CharacterStateType.run) ? true : false;
    }
    public void Revival()
    {
        onRevival?.Invoke();
        SetHealth(maxHealth);
    }

    public void TakeDamage(Damage damage)
    {
        var totalDamage = damage.GetTotalDamage(characterMainController.gameObject);
        var result = Mathf.Max(currentHealth - totalDamage, 0);
        SetHealth(result);
    }
}
