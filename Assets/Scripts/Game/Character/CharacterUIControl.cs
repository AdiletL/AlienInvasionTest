using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LookAtCamera))]
public class CharacterUIControl : MonoBehaviour, IControl
{
    [SerializeField] protected Image healthBar;
    [SerializeField] protected TextMeshProUGUI healthTxt;

    public IController iController { get; set; }

    private CharacterMainController characterController;

    public virtual void Initialize(IController controller)
    {
        iController = controller;
        characterController = controller as CharacterMainController;
        if (characterController == null)
        {
            enabled = false;
            return;
        }
    }
    protected virtual void OnEnable()
    {
        characterController.GetControl<CharacterHealthCotrol>().onSetHealth += OnSetHealth;

    }
    protected virtual void OnDisable()
    {
        characterController.GetControl<CharacterHealthCotrol>().onSetHealth -= OnSetHealth;

    }

    protected virtual void OnSetHealth(int maxHealth, int currentHealth)
    {
        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
        healthTxt.text = ((currentHealth * 100) / maxHealth).ToString() + "%";
    }
}
