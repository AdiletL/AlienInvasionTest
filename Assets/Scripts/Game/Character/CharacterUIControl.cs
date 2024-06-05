using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIControl : MonoBehaviour, IControl
{
    [SerializeField] protected Image healthBar;

    public IController iController { get; set; }

    private CharacterMainController characterController;

    public void Initialize(IController controller)
    {
        iController = controller;
        characterController = controller as CharacterMainController;
        if (characterController == null)
        {
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        if (characterController == null)
        {
            enabled = false;
            return;
        }
        InitializeEvent();
    }
    protected virtual void InitializeEvent()
    {
        characterController.GetControl<CharacterHealthCotrol>().onSetHealth += OnSetHealth;
    }
    
    protected virtual void OnSetHealth(int maxHealth, int currentHealth)
    {
        healthBar.fillAmount = (currentHealth / maxHealth);
    }

    private void OnDestroy()
    {
        DeInitializeEvent();
    }
    protected virtual void DeInitializeEvent()
    {
        characterController.GetControl<CharacterHealthCotrol>().onSetHealth -= OnSetHealth;
    }
}
