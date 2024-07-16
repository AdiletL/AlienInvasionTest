using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterMainController
{
    private void Start()
    {
        SwitchState(CharacterStateType.idle);
        FindObjectOfType<CinemachineVirtualCamera>().Follow = transform;
    }
}
