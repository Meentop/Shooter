using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandWithWeapon : MonoBehaviour, ISelectableItem
{
    public SelectableItems ItemType => SelectableItems.Weapon;

    public string Text => Bought ? "Press E to buy" : "Press E to equip";

    public bool Bought { get; private set; }

    [SerializeField] private StandWeapon standWeapon;

    public void SetStandWeapon(StandWeapon standWeapon)
    {
        this.standWeapon = standWeapon;
    }

    public void OnSelect(Player player)
    {

    }
}
