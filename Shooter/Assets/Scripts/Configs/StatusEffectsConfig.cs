using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Status effect config")]
public class StatusEffectsConfig : ScriptableObject
{
    public Sprite[] sprites;
    public Color[] colors;
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
