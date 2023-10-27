using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "ScriptableObjects/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    [SerializeField] private List<WeaponCharacteristics> characteristics = new List<WeaponCharacteristics>();
    [SerializeField] private List<StandWeapon> standWeapons = new List<StandWeapon>();
    [SerializeField] private List<Weapon> weapons = new List<Weapon>();
    [HideInInspector] public List<WeaponCharacteristics> Characteristics { get => characteristics; }
    [HideInInspector] public List<StandWeapon> StandWeapons { get => standWeapons; }
    [HideInInspector] public List<Weapon> Weapons { get => weapons; }

    public int GetIndex(WeaponCharacteristics characteristics)
    {
        foreach (var weapon in this.characteristics)
        {
            if (weapon.WeaponName == characteristics.WeaponName)
                return this.characteristics.IndexOf(weapon);
        }
        return -1;
    }
}
