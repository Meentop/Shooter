using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceToIncreaseStatusEffect : WeaponModuleBehaviour
{
    [SerializeField] private StatusEffect statusEffect;
    [SerializeField] private int[] chance;
    [SerializeField] private float increase;

    public override DamageData ApplyBehaviour(DamageData damageData, InfoForWeaponModule info)
    {
        if(info.enemyStatusEffects.ContainsKey(statusEffect))
        {
            if (1 == Random.Range(1, chance[info.lvl])) 
            {
                if (!damageData.AddStatusEffects.ContainsKey(statusEffect))
                    damageData.AddStatusEffects.Add(statusEffect, (int)(info.enemyStatusEffects[statusEffect] * increase));
                else
                    damageData.AddStatusEffects[statusEffect] += (int)(info.enemyStatusEffects[statusEffect] * increase);
            }
        }
        return damageData;
    }

    public override string GetDescription(int lvl)
    {
        return $"+{chance[lvl]}% chance to double {statusEffect} on the enemy";
    }
}
