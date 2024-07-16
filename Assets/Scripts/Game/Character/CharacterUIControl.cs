using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LookAtCamera))]
public class CharacterUIControl : MainControl
{
    [SerializeField] private CharacterMainController characterMainController;

    [Space]
    [SerializeField] protected Image healthBar;
    [SerializeField] protected TextMeshProUGUI healthTxt;

    protected override void SetController()
    {
        iController = characterMainController;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        characterMainController.GetControl<CharacterHealthCotrol>().onSetHealth += OnSetHealth;

    }
    protected override void OnDisable()
    {
        base.OnDisable();
        characterMainController.GetControl<CharacterHealthCotrol>().onSetHealth -= OnSetHealth;
    }

    protected virtual void OnSetHealth(int maxHealth, int currentHealth)
    {
        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
        healthTxt.text = ((currentHealth * 100) / maxHealth).ToString() + "%";
    }
}
