using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Terminal : MonoBehaviour, ISelectableItem
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private WeaponConfig weaponConfig;
    [SerializeField] private int[] golds;
    [SerializeField] private WeaponModuleConfig weaponModuleConfig;
    [SerializeField] private BionicModuleConfig bionicModuleConfig;
    [SerializeField] private ActiveSkillConfig activeSkillConfig;
    [SerializeField] private RoomAwardsConfig roomAwardsConfig;

    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private Room room;

    private Player player;

    public SelectableItems ItemType => SelectableItems.TerminalButton;

    public string Text => "E";

    public void OnSelect(Player player)
    {
        this.player = player;
    }

    public List<string> GetButtonsName(TerminalButtonType type)
    {
        List<string> names = new List<string>();
        switch (type)
        {
            case TerminalButtonType.Enemy:
                foreach (var enemy in enemies)
                {
                    names.Add(enemy.name);
                }
                break;
            case TerminalButtonType.Weapon:
                foreach (var weapon in weaponConfig.Characteristics)
                {
                    names.Add(weapon.name);
                }
                break;
            case TerminalButtonType.Gold:
                foreach (var count in golds)
                {
                    names.Add(count.ToString());
                }
                break;
            case TerminalButtonType.WeaponModule:
                foreach (var module in weaponModuleConfig.Modules)
                {
                    names.Add(module.name);
                }
                break;
            case TerminalButtonType.BionicModule:
                foreach (var module in bionicModuleConfig.Modules)
                {
                    names.Add(module.name);
                }
                break;
            case TerminalButtonType.ActiveSkill:
                foreach (var skill in activeSkillConfig.ActiveSkills)
                {
                    names.Add(skill.name);
                }
                break;
            case TerminalButtonType.Award:
                foreach (var award in roomAwardsConfig.Awards)
                {
                    if (award != null) 
                        names.Add(award.name);
                    else
                        names.Add("");
                }
                break;
        }
        return names;
    }

    public void ApplyTerminalButton(TerminalButtonType type, int index)
    {
        switch (type)
        {
            case TerminalButtonType.Enemy:
                Instantiate(enemies[index], enemySpawnPoint.position, Quaternion.identity);
                break;
            case TerminalButtonType.Weapon:
                Weapon weapon = Instantiate(weaponConfig.Weapons[index]);
                Destroy(player.GetSelectedWeapon().gameObject);
                player.AddWeapon(weapon, player.GetSelectedWeaponSlot());
                weapon.ConectToPlayer();
                player.SetCurrentWeapon();
                break;
            case TerminalButtonType.Gold:
                player.Gold.Add(golds[index]);
                break;
            case TerminalButtonType.WeaponModule:
                player.AddFreeModule(weaponModuleConfig.Modules[index]);
                break;
            case TerminalButtonType.BionicModule:
                player.AddFreeModule(bionicModuleConfig.Modules[index]);
                break;
            case TerminalButtonType.ActiveSkill:
                player.AddActiveSkill(activeSkillConfig.ActiveSkills[index]);
                break;
            case TerminalButtonType.Award:
                room.SpawnAward((AwardType)index);
                break;
        }
    }

}

public enum TerminalButtonType
{
    Enemy,
    Weapon,
    Gold,
    WeaponModule,
    BionicModule,
    ActiveSkill,
    Award
}