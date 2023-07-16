using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAward : MonoBehaviour
{
    [SerializeField] private Transform[] stands;
    [SerializeField] private List<GameObject> weaponHolders;

    private void Start()
    {
        foreach (var stand in stands)
        {
            int randomNumber = Random.Range(0, weaponHolders.Count);
            GameObject randWeaponHolder = weaponHolders[randomNumber];
            weaponHolders.RemoveAt(randomNumber);
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
