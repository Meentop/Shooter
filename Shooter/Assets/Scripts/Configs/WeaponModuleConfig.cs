using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponModuleConfig", menuName = "ScriptableObjects/WeaponModuleConfig")]
public class WeaponModuleConfig : ScriptableObject
{
    [SerializeField] private List<WeaponModule> modules = new List<WeaponModule>();
    [HideInInspector] public List<WeaponModule> Modules { get => modules; }

    public int GetIndex(WeaponModule module)
    {
        foreach (WeaponModule module2 in modules)
        {
            if (module2.GetTitle(0) == module.GetTitle(0))
                return modules.IndexOf(module2);
        }
        return -1;
    }
}