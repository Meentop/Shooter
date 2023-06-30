using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceToIncreaseStatusEffect : ModifierBehaviour
{
    [SerializeField] private StatusEffect statusEffect;
    [SerializeField] private int chance;
    [SerializeField] private float increase;

    public override DamageData ApplyBehaviour(DamageData damageData, EnemyHealth enemy)
    {
        if(enemy.HasStatusEffect(statusEffect))
        {
            if (1 == Random.Range(1, chance)) 
            {
                if (!damageData.AddStatusEffects.ContainsKey(statusEffect))
                    damageData.AddStatusEffects.Add(statusEffect, (int)(enemy.GetStatusEffectValue(statusEffect) * increase));
                else
                    damageData.AddStatusEffects[statusEffect] += (int)(enemy.GetStatusEffectValue(statusEffect) * increase);
                print("chance");
            }
        }
        return damageData;
    }
}
