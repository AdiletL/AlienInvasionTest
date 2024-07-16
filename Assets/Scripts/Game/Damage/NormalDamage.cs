using System.Collections;
using UnityEngine;


public class NormalDamage : Damage
{
    public NormalDamage(DamageType damageType, DamageCalculationType damageCalculationType, int amount) : base(damageType, damageCalculationType, amount)
    {

    }
    public override void Apply(GameObject target)
    {
        
    }
}
