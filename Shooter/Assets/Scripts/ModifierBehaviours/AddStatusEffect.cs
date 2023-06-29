using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStatusEffect : ModifierBehaviour
{
    [SerializeField] private StatusEffect statusEffect;
    [SerializeField] private int value;

    public override DamageData ApplyBehaviour(DamageData damageData)
    {
        if (!damageData.AddStatusEffects.ContainsKey(statusEffect))
            damageData.AddStatusEffects.Add(statusEffect, value);
        else
            damageData.AddStatusEffects[statusEffect] += value;
        return damageData;
    }
}
