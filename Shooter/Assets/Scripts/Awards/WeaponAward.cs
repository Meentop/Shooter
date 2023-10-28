using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAward : MonoBehaviour
{
    [SerializeField] private StandWithWeapon[] stands;
    [SerializeField] private Transform[] holders;
    [SerializeField] private WeaponConfig weaponConfig;

    private void Start()
    {
        List<StandWeapon> standWeapons = new List<StandWeapon>();
        standWeapons.AddRange(weaponConfig.StandWeapons);
        for (int i = 0; i < stands.Length; i++)
        {
            int randomNumber = Random.Range(1, standWeapons.Count);
            StandWeapon randStandWeapon = standWeapons[randomNumber];
            standWeapons.RemoveAt(randomNumber);
            StandWeapon standWeapon = Instantiate(randStandWeapon, holders[i]);
            stands[i].SetStandWeapon(randStandWeapon);
            //weapon.ConnectToStand(stand);
        }
    }

    public void DeleteOtherWeapons(Transform thisStand)
    {
        foreach (var stand in stands)
        {
            if (stand != thisStand && stand.transform.childCount > 0)
                Destroy(stand.GetComponentInChildren<Weapon>().gameObject);
        }
    }
}
