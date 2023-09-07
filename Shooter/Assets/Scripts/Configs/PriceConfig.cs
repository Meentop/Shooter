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

    [SerializeField] private int weaponUpgrade;
    [SerializeField] private int weaponModuleUpgrade;
    [SerializeField] private int bionicModuleUpgrade;
}
