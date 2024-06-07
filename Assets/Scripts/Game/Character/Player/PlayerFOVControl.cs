using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOVControl : CharacterFOVControl
{
    private PlayerMainController playerMainController;

    [SerializeField] private GameObject radiusFOV;


    public override void Initialize(IController controller)
    {
        base.Initialize(controller);
        playerMainController = iController as PlayerMainController;
        radiusFOV.transform.localScale = Vector3.one * (playerMainController.so_CharacterConfig.radiusFOV * 2);
        radiusFOV.SetActive(false);
    }

    private void OnEnable()
    {
        playerMainController.GetControl<PlayerAttackControl>().OnAttack += OnAttack;
    }
    private void OnDisable()
    {
        playerMainController.GetControl<PlayerAttackControl>().OnAttack -= OnAttack;
    }

    private void OnAttack(bool isAttack) => radiusFOV.SetActive(isAttack);
}
