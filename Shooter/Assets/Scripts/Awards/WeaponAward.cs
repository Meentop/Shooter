using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAward : MonoBehaviour
{
    [SerializeField] private Transform[] stands;
    [SerializeField] private WeaponConfig weaponConfig;

    private void Start()
    {
        List<GameObject> weapons = new List<GameObject>();
        weapons.AddRange(weaponConfig.Weapons);
        foreach (var stand in stands)
        {
            int randomNumber = Random.Range(1, weapons.Count);
            GameObject randWeaponHolder = weapons[randomNumber];
            weapons.RemoveAt(randomNumber);
            Weapon weapon = Instantiate(randWeaponHolder, stand).GetComponentInChildren<Weapon>();
            weapon.ConnectToStand(stand);
        }
    }

    public void DeleteOtherWeapons(Transform thisStand)
    {
        foreach (var stand in stands)
        {
            if (stand != thisStand && stand.childCount > 0)
                Destroy(stand.GetComponentInChildren<Weapon>().gameObject);
        }
    }
}
