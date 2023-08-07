using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubWeaponStands : MonoBehaviour
{
    [SerializeField] private GameObject standPrefab;
    [SerializeField] private float standDistance;
    [SerializeField] private int standCount;
    private List<Transform> stands = new List<Transform>();
    [SerializeField] private List<GameObject> weaponHolders;

    private void Start()
    {
        for (int i = 0; i < standCount; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + (standDistance * i), transform.position.y, transform.position.z);
            stands.Add(Instantiate(standPrefab, pos, Quaternion.identity, transform).transform);
        }
        foreach (var stand in stands)
        {
            int randomNumber = Random.Range(0, weaponHolders.Count);
            GameObject randWeaponHolder = weaponHolders[randomNumber];
            weaponHolders.RemoveAt(randomNumber);
            Weapon weapon = Instantiate(randWeaponHolder, stand).GetComponentInChildren<Weapon>();
            weapon.ConnectToStand(stand);
            weapon.SetBought();
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
