using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponModule : Module
{
    [SerializeField] private List<WeaponModuleBehaviour> behaviours;

    public DamageData ApplyBehaviours(DamageData damageData, InfoForWeaponModule info)
    {
        foreach (var behaviour in behaviours)
        {
            damageData = behaviour.ApplyBehaviour(damageData, info);
        }
        return damageData;
    }

    public WeaponModuleSave GetSave()
    {
        WeaponModuleSave weaponModuleSave = new WeaponModuleSave
        { 
            number = Number,
            level = level
        };
        return weaponModuleSave;
    }
}
