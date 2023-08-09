using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "ScriptableObjects/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    public List<GameObject> weapons;

    [ContextMenu("Set numbers")]
    public void SetNumbers()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].GetComponentInChildren<Weapon>().Number = i;
            Debug.Log(weapons[i].GetComponentInChildren<Weapon>().Number);
        }
    }
}
