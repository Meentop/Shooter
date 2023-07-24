using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponModule : Module
{
    [SerializeField] private List<WeaponModuleBehaviour> behaviours;

    public DamageData ApplyBehaviours(DamageData damageData, EnemyHealth enemy)
    {
        foreach (var behaviour in behaviours)
        {
            damageData = behaviour.ApplyBehaviour(damageData, enemy);
        }
        return damageData;
    }
}
