using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "ScriptableObjects/StatusEffectConfig")]
public class StatusEffectsConfig : ScriptableObject
{
    public Sprite[] sprites;
    public Color[] colors;
    public float statsuEffectDeltaTime;
}

public enum StatusEffect
{
    Poison,
    Freeze,
    Burn,
    Electrization,
    Dark,
    Curse
}
