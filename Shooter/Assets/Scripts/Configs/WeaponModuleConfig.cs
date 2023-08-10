using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponModuleConfig", menuName = "ScriptableObjects/WeaponModuleConfig")]
public class WeaponModuleConfig : ScriptableObject
{
    public List<WeaponModule> modules;
}