using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponModifierHolderUI : ModifierHolderUI
{
    [SerializeField] private Color activeColor, unactiveColor;
    private Image _image;
    private Weapon _weapon;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void SetWeapon(Weapon weapon)
    {
        _weapon = weapon;
    }

    public override void OnEnter()
    {
        _image.color = activeColor;
    }

    public override void OnExit()
    {
        _image.color = unactiveColor;
    }

    public override void SetPosition(Transform modifier)
    {
        modifier.SetParent(transform);
        modifier.position = transform.position;
    }

    public override void AddModifier(ModifierUI modifier)
    {
        _weapon.AddModifier(modifier.modifier);
    }

    public override bool CanAddModifier()
    {
        return transform.childCount == 0;
    }

    public override void RemoveModifier(ModifierUI modifier)
    {
        _weapon.RemoveModifier(modifier.modifier);
    }
}
