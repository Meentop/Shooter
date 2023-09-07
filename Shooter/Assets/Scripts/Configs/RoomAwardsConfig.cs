using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomAwardConfig", menuName = "ScriptableObjects/RoomAwardConfig")]
public class RoomAwardsConfig : ScriptableObject
{
    [SerializeField] private GameObject[] awards;

    [HideInInspector] public GameObject[] Awards { get => awards; }
}

public enum AwardType
{
    Start,
    Gold,
    Health,
    Weapon,
    WeaponModule,
    BionicModule,
    WeaponUpgrade,
    ModuleUpgrade,
    ActiveSkill,
    Portal
}
