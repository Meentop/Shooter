using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyStatusEffectDamage : ModifierBehaviour
{
    [SerializeField] private StatusEffect statusEffect;
    [SerializeField] private float statusEffectDamage;

    public override DamageData ApplyBehaviour(DamageData damageData, EnemyHealth enemy)
    {
        if (!damageData.AddStatusEffects.ContainsKey(statusEffect))
            damageData.AddStatusEffects.Add(statusEffect, (int)(damageData.Damage * statusEffectDamage));
        else
            damageData.AddStatusEffects[statusEffect] += (int)(damageData.Damage * statusEffectDamage);
        damageData.SetDamage(0);
        return damageData;
    }
}
