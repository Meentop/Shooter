using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponModuleHolderUI : ModuleHolderUI
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

    public override void SetPosition(Transform module)
    {
        module.SetParent(transform);
        module.position = transform.position;
    }

    public override void AddModule(ModuleUI module)
    {
        _weapon.AddModule(module.Module as WeaponModule);
    }

    public override bool CanAddModule(ModuleUI module)
    {
        return transform.childCount == 0 && module.Module.GetType() == typeof(WeaponModule);
    }

    public override void RemoveModule(ModuleUI module)
    {
        _weapon.RemoveModule(module.Module as WeaponModule);
    }
}
