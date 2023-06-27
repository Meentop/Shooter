using UnityEngine;
using System.Collections.Generic;

public interface IDamageReceiver
{
    void GetDamage(DamageData damageData);
}

public struct DamageData
{
    public readonly int Damage;
    //public readonly Dictionary<StatusEffect, int> statusEffects;

    public DamageData(int damage)
    {
        Damage = damage;
    }
}

