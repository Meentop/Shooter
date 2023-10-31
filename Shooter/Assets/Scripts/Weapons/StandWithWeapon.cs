using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandWithWeapon : MonoBehaviour, ISelectableItem
{
    public SelectableItems ItemType => SelectableItems.Weapon;

    public string Text => StandWeapon.Bought ? "Press E to buy" : "Press E to equip";

    public StandWeapon StandWeapon { get; private set; }
    [SerializeField] private Transform holder;
    [SerializeField] private WeaponConfig weaponConfig;
    [SerializeField] private Collider collider1;

    public void SetStandWeapon(StandWeapon standWeapon)
    {
        StandWeapon = standWeapon;
    }

    public void SetBoughtStandWeapon(int index)
    {
        if (holder.childCount > 0)
            Destroy(holder.GetChild(0).gameObject);
        StandWeapon = Instantiate(weaponConfig.StandWeapons[index], holder);
        StandWeapon.SetBought();
        if (index == 0)
            collider1.enabled = false;
        else
            collider1.enabled = true;
    }

    public WeaponCharacteristics GetWeaponCharacteristics()
    {
        return StandWeapon.GetWeaponCharacteristics();
    }

    public void OnSelect(Player player)
    {

    }
}
