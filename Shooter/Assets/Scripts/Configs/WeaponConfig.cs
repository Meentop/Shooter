using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "ScriptableObjects/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    public List<GameObject> weapons;
}
