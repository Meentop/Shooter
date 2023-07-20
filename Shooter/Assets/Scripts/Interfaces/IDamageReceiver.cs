using UnityEngine;
using System.Collections.Generic;

public interface IDamageReceiver
{
    void GetDamage(DamageData damageData);
}

public class DamageData 
{
    public DamageData(int damage = 0, Dictionary<StatusEffect, int> addStatusEffects = null, Dictionary<StatusEffect, int> statusEffectsDamage = null)
    {
        Damage = damage;
        AddStatusEffects = addStatusEffects;
        StatusEffectsDamage = statusEffectsDamage;
        AddStatusEffects ??= new Dictionary<StatusEffect, int>();
        StatusEffectsDamage ??= new Dictionary<StatusEffect, int>();
    }

    public int Damage { get; private set; }
    public Dictionary<StatusEffect, int> AddStatusEffects { get; private set; }
    public Dictionary<StatusEffect, int> StatusEffectsDamage { get; private set; }

    public void SetDamage(int damage) => Damage = damage;
}

