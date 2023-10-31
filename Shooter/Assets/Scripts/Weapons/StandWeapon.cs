using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandWeapon : MonoBehaviour
{
    [SerializeField] private WeaponCharacteristics characteristics;
    public int Level { get; private set; } = 0;
    public bool Bought { get; private set; } = false;
    public WeaponCharacteristics GetWeaponCharacteristics() => characteristics;

    public void SetBought()
    {
        Bought = true;
    }
}
