using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomAwardConfig", menuName = "ScriptableObjects/RoomAwardConfig")]
public class RoomAwardsConfig : ScriptableObject
{
    public GameObject[] awards;
}

public enum AwardType
{
    None,
    Gold,
    Health,
    Weapon,
    WeaponModule,
    BionicModule,
    WeaponUpgrade,
    ModuleUpgrade,
    ActiveSkill
}
