using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandWeapon : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private WeaponCharacteristics characteristics;

    public Weapon GetWeapon() => weapon;
    public WeaponCharacteristics GetWeaponCharacteristics() => characteristics;
}
