using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncriseDamageByStatusEffect : ModifierBehaviour
{
    [SerializeField] private StatusEffect statusEffect;
    [SerializeField] private float increase;

    public override DamageData ApplyBehaviour(DamageData damageData, EnemyHealth enemy)
    {
        if (enemy.HasStatusEffect(statusEffect))
            damageData.SetDamage((int)(damageData.Damage * increase));
        return damageData;
    }
}
