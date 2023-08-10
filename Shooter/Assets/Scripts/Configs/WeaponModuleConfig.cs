using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponModuleConfig", menuName = "ScriptableObjects/WeaponModuleConfig")]
public class WeaponModuleConfig : ScriptableObject
{
    [SerializeField] private List<WeaponModule> modules = new List<WeaponModule>();
    [HideInInspector] public List<WeaponModule> Modules { get => modules; }
}