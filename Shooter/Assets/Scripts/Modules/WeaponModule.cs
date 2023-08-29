using UnityEngine;

public class WeaponModule : Module
{
    [SerializeField] private WeaponModuleBehaviour behaviour;

    public DamageData ApplyBehaviour(DamageData damageData, InfoForWeaponModule info)
    {
        return behaviour.ApplyBehaviour(damageData, info);
    }

    public override string GetDescription(int lvl)
    {
        return behaviour.GetDescription(lvl);
    }

    public WeaponModuleSave GetSave()
    {
        WeaponModuleSave weaponModuleSave = new WeaponModuleSave
        {
            number = number,
            level = Level
        };
        return weaponModuleSave;
    }
}
