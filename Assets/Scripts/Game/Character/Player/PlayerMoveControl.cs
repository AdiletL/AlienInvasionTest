﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoveControl : CharacterMoveControl
{
    private PlayerController playerComponent;
    private CharacterController characterController;

    private Vector3 currentDirection;

    private float movementSpeed = 5;
    private float rotateSpeed = 7;
    private float gravity = 4;

    private bool isRunEnabled;

    public override void Initialize()
    {
        base.Initialize();
        characterController = GetComponent<CharacterController>();

        var playerConfig = (SO_PlayerConfig)playerComponent.so_CharacterConfig;
        movementSpeed = playerConfig.movementSpeed;
        rotateSpeed = playerConfig.rotateSpeed;
        gravity = playerConfig.gravity;
    }
    protected override void SetController()
    {
        base.SetController();
        playerComponent = (PlayerController)iController;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        playerComponent.GetControl<PlayerControl>().OnSwipe += OnSwipe;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        playerComponent.GetControl<PlayerControl>().OnSwipe -= OnSwipe;
    }

    private void OnSwipe(Vector3 direction) => currentDirection = direction;

    public override void Move()
    {
        RotatePlayer();
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (!isRunEnabled) return;

        if (currentDirection == Vector3.zero) playerComponent.SwitchState(CharacterStateType.idle);
        else playerComponent.SwitchState(CharacterStateType.run);

        Vector3 move = new Vector3(currentDirection.x, 0, currentDirection.z);
        move.y += gravity;
        characterController.Move(move * movementSpeed * Time.deltaTime);
    }
    private void RotatePlayer()
    {
        if (currentDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(currentDirection.x, 0, currentDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    protected override void OnSwitchController(CharacterStateType state)
    {
        isRunEnabled = state.HasFlag(CharacterStateType.run) ? true : false;
    }
}
