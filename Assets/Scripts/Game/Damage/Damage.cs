using System.Collections;
using UnityEngine;


public abstract class Damage
{
    private DamageType damageType;
    private DamageCalculationType calculationType;
    private int amount;

    public Damage(DamageType damageType, DamageCalculationType damageCalculationType, int amount)
    {
        this.damageType = damageType;
        this.calculationType = damageCalculationType;
        this.amount = amount;
    }

    public virtual int GetTotalDamage(GameObject target)
    {
        int totalDamage = amount;
        return totalDamage;
    }
    public abstract void Apply(GameObject target);
}
