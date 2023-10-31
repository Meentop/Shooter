using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveLoadManager : MonoBehaviour
{
    [SerializeField] private ActiveSkillConfig activeSkillConfig;
    [SerializeField] private WeaponConfig weaponConfig;
    [SerializeField] private WeaponModuleConfig weaponModuleConfig;
    [SerializeField] private BionicModuleConfig bionicModuleConfig;

    private Player _player;

    public void Init(Player player)
    {
        _player = player;
    }

    public List<WeaponSave> GetWeaponSaves()
    {
        List<WeaponSave> weapons = new List<WeaponSave>();
        foreach (var weapon in _player.GetWeapons())
        {
            weapons.Add(weapon.GetSave());
        }
        return weapons;
    }

    public List<WeaponModuleSave> GetFreeWeaponModulesSave()
    {
        List<WeaponModuleSave> modules = new List<WeaponModuleSave>();
        foreach (var module in _player.GetFreeWeaponModules())
        {
            modules.Add(module.GetSave());
        }
        return modules;
    }

    public List<BionicModuleSave> GetInstalledBionicModulesSave()
    {
        List<BionicModuleSave> modules = new List<BionicModuleSave>();
        foreach (var module in _player.GetInstalledBionicModules())
        {
            modules.Add(module.GetSave());
        }
        return modules;
    }

    public List<BionicModuleSave> GetFreeBionicModulesSave()
    {
        List<BionicModuleSave> modules = new List<BionicModuleSave>();
        foreach (var module in _player.GetFreeBionicModules())
        {
            modules.Add(module.GetSave());
        }
        return modules;
    }

    public int GetActiveSkillNumber()
    {
        return activeSkillConfig.GetIndex(_player.GetActiveSkill());
    }



    public void LoadWeapons(List<WeaponSave> weaponSaves)
    {
        for (int i = 0; i < weaponSaves.Count; i++)
        {
            Weapon weapon = _player.AddWeaponFromSave(weaponSaves[i].number, i);
            weapon.SetLevel(weaponSaves[i].level);
            foreach (var module in weaponSaves[i].modules)
            {
                WeaponModule weaponModule = weaponModuleConfig.Modules[module.number];
                weaponModule.SetLevel(module.level);
                weapon.AddModule(weaponModule);
            }
        }
        _player.SetCurrentWeapon();
    }

    public void LoadFreeWeaponModules(List<WeaponModuleSave> modules)
    {
        foreach (var module in modules)
        {
            WeaponModule weaponModule = weaponModuleConfig.Modules[module.number];
            weaponModule.SetLevel(module.level);
            _player.AddFreeModule(weaponModule);
        }
    }

    public void LoadActiveSkill(int number)
    {
        _player.AddActiveSkill(activeSkillConfig.ActiveSkills[number]);
    }

    public void LoadInstalledBionicModules(List<BionicModuleSave> modules)
    {
        foreach (var module in modules)
        {
            BionicModule bionicnModule = bionicModuleConfig.Modules[module.number];
            bionicnModule.SetLevel(module.level);
            _player.AddInstalledBionicModule(bionicnModule);
        }
        _player.SetCharacteristics();
    }

    public void LoadFreeBionicModules(List<BionicModuleSave> modules)
    {
        foreach (var module in modules)
        {
            BionicModule bionicnModule = bionicModuleConfig.Modules[module.number];
            bionicnModule.SetLevel(module.level);
            _player.AddFreeModule(bionicnModule);
        }
    }
}
