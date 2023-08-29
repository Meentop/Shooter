using UnityEngine;
using System.Collections.Generic;

public interface IDamageReceiver
{
    void GetDamage(DamageData damageData);
}

public class DamageData 
{
    public DamageData(int damage = 0, int critChance = 0, float critDamageMultiplier = 3, Dictionary<StatusEffect, int> addStatusEffects = null, Dictionary<StatusEffect, int> statusEffectsDamage = null)
    {
        Damage = damage;
        CritChance = critChance;
        CritDamageMultiplier = critDamageMultiplier;
        AddStatusEffects = addStatusEffects;
        StatusEffectsDamage = statusEffectsDamage;
        AddStatusEffects ??= new Dictionary<StatusEffect, int>();
        StatusEffectsDamage ??= new Dictionary<StatusEffect, int>();
    }

    public int Damage { get; private set; }
    public int CritChance { get; private set; }
    public float CritDamageMultiplier { get; private set; }
    public Dictionary<StatusEffect, int> AddStatusEffects { get; private set; }
    public Dictionary<StatusEffect, int> StatusEffectsDamage { get; private set; }

    public void SetDamage(int damage) => Damage = damage;
    public void AddCritChance(int critChance) => CritChance += critChance;
    public void AddCritDamage(float addCritDamage) => CritDamageMultiplier += addCritDamage;
}

