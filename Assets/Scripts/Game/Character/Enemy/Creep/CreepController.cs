using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreepHealthControl))]
public class CreepController : EnemyController
{
    protected override EnemyData CreateData()
    {
        return new CreepData();
    }
}

public class CreepData : EnemyData
{

}