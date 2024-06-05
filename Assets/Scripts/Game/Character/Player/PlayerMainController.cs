using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainController : CharacterMainController
{


    private void Start()
    {
        SwitchState(CharacterStateType.idle);
    }

}
