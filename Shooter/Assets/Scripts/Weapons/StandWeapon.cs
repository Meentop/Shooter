using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandWeapon : MonoBehaviour, ISelectableItem
{
    public SelectableItems ItemType => SelectableItems.Weapon;

    public string Text => Bought ? "Press E to buy" : "Press E to equip";

    public bool Bought { get; private set; }

    [SerializeField] private Weapon weapon;
    [SerializeField] private WeaponCharacteristics characteristics;

    public Weapon GetWeapon() => weapon;
    public WeaponCharacteristics GetWeaponCharacteristics() => characteristics;

    public void OnSelect(Player player)
    {

    }
}
