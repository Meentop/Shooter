using UnityEngine;
using System.Collections.Generic;

public interface IDamageReceiver
{
    void GetDamage(DamageData damageData);
}

public struct DamageData
{
    public readonly int Damage;
    public readonly Dictionary<StatusEffect, int> AddStatusEffects;
    public readonly Dictionary<StatusEffect, int> StatusEffectsDamage;

    public DamageData(int damage, Dictionary<StatusEffect, int> addStatysEffects = null, Dictionary<StatusEffect, int> statuseffectsDamage = null)
    {
        Damage = damage;
        AddStatusEffects = addStatysEffects;
        StatusEffectsDamage = statuseffectsDamage;
    }
}

