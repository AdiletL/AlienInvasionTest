using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOVControl : CharacterFOVControl
{
    private PlayerController playerController;

    [SerializeField] private GameObject radiusFOV;

    public override void Initialize()
    {
        base.Initialize();
        radiusFOV.transform.localScale = Vector3.one * (playerController.so_CharacterConfig.radiusFOV * 2);
        radiusFOV.SetActive(false);
    }
    protected override void SetController()
    {
        base.SetController();
        playerController = (PlayerController)iController;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerController.GetControl<PlayerAttackControl>().OnAttack += OnAttack;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        playerController.GetControl<PlayerAttackControl>().OnAttack -= OnAttack;
    }

    private void OnAttack(bool isAttack) => radiusFOV.SetActive(isAttack);
}
