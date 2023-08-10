using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "ScriptableObjects/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    [SerializeField] private List<GameObject> weapons = new List<GameObject>();
    [HideInInspector] public List<GameObject> Weapons { get => weapons; }
}
