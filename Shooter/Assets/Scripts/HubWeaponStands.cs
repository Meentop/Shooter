using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubWeaponStands : MonoBehaviour
{
    [SerializeField] private StandWithWeapon standPrefab;
    [SerializeField] private float standDistance;
    [SerializeField] private int standCount;
    private List<StandWithWeapon> stands = new List<StandWithWeapon>();
    [SerializeField] private WeaponConfig weaponConfig;

    private void Start()
    {
        for (int i = 0; i < standCount; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + (standDistance * i), transform.position.y, transform.position.z);
            stands.Add(Instantiate(standPrefab, pos, Quaternion.identity, transform));
        }

        List<StandWeapon> standWeapons = new List<StandWeapon>();
        standWeapons.AddRange(weaponConfig.StandWeapons);
        for (int i = 0; i < stands.Count; i++)
        {
            int randomNumber = Random.Range(1, standWeapons.Count);
            standWeapons.RemoveAt(randomNumber);
            stands[i].SetBoughtStandWeapon(randomNumber);
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
