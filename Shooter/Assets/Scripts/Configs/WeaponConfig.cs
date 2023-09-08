using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "ScriptableObjects/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    [SerializeField] private List<GameObject> weapons = new List<GameObject>();
    [HideInInspector] public List<GameObject> Weapons { get => weapons; }

    public int GetIndex(Weapon weapon)
    {
        foreach (GameObject weapon2 in weapons)
        {
            if (weapon2.GetComponentInChildren<Weapon>().GetName() == weapon.GetName())
                return weapons.IndexOf(weapon2);
        }
        return -1;
    }
}
