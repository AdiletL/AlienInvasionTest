using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OgreHealthControl))]
public class OgreController : EnemyController
{
    protected override EnemyData CreateData()
    {
        return new OgreData();
    }
}

public class OgreData : EnemyData
{

}