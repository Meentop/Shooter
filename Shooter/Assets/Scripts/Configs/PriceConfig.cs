using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PriceConfig", menuName = "ScriptableObjects/PriceConfig")]
public class PriceConfig : ScriptableObject
{
    [SerializeField] private float[] levelMultipliers;

    [SerializeField] private int[] weapons;
    [SerializeField] private int weaponModule;
    [SerializeField] private int bionicModule;
    [SerializeField] private int[] activeSkill;

    [SerializeField] private int[] weaponUpgrades;
    [SerializeField] private int weaponModuleUpgrade;
    [SerializeField] private int bionicModuleUpgrade;

    [SerializeField] private int healingPrice;

    public int GetWeaponPrice(int weapon, int lvl)
    {
        return (int)(weapons[weapon] * levelMultipliers[lvl]);
    }

    public int GetWeaponModulePrice(int lvl)
    {
        return (int)(weaponModule * levelMultipliers[lvl]);
    }

    public int GetBionicModulePrice(int lvl)
    {
        return (int)(bionicModule * levelMultipliers[lvl]);
    }

    public int GetActiveSkillPrice(int skill)
    {
        return activeSkill[skill];
    }

    public int GetHealingPrice(int lvl)
    {
        return (int)(healingPrice * levelMultipliers[lvl]);
    }

    public int GetWeaponUpgradePrice(int weapon, int lvl)
    {
        return (int)(weaponUpgrades[weapon] * levelMultipliers[lvl]);
    }

    public int GetWeaponModuleUpgradePrice(int lvl)
    {
        return (int)(weaponModuleUpgrade * levelMultipliers[lvl]);
    }

    public int GetBionicModuleUpgradePrice(int lvl)
    {
        Debug.Log("bionic upgrade");
        return (int)(bionicModuleUpgrade * levelMultipliers[lvl]);
    }
}
