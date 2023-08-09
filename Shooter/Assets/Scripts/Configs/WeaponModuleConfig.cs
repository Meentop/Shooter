using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponModuleConfig", menuName = "ScriptableObjects/WeaponModuleConfig")]
public class WeaponModuleConfig : ScriptableObject
{
    public List<WeaponModule> modules;

    [ContextMenu("Set numbers")]
    public void SetNumbers()
    {
        for (int i = 0; i < modules.Count; i++)
        {
            modules[i].GetComponent<Module>().Number = i;
            Debug.Log(modules[i].GetComponent<Module>().Number);
        }
    }
}